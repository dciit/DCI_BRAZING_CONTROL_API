using BrazingControlAPI.Contexts;
using BrazingControlAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace BrazingControlAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly DBDCI _contextDCI;
        private readonly DBHRM _contextHRM;
        private readonly DBSCM _contextSCM;
        public WeatherForecastController(DBDCI contextDCI, DBHRM contextHRM, DBSCM contextSCM)
        {
            _contextDCI = contextDCI;
            _contextHRM = contextHRM;
            _contextSCM = contextSCM;
        }

        [HttpGet]
        [Route("/brazing/linecontrol")]
        public IActionResult GetLineControl()
        {
            var content = _contextDCI.TrCourses.Where(x => x.ExpireStatus == true).OrderBy(x => x.CourseCode).ToList();
            return Ok(content);
        }
        [HttpGet]
        [Route("/brazing/user/{lineControl}")]
        public IActionResult GetBrzingUser(string lineControl)
        {
            var content = (from course in _contextDCI.TrCourses
                           where course.ExpireStatus == true
                           select new
                           {
                               course.CourseCode,
                               course.CourseName,
                               schedule = _contextDCI.TrSchedules.Where(x => x.CourseId == course.Id.ToString() && x.Status == "ACTIVE").OrderByDescending(x => x.ScheduleStart).FirstOrDefault()
                           }).ToList();
            if (lineControl != "" && lineControl != "-")
            {
                content = content.Where(x => x.CourseCode == lineControl).ToList();
            }
            DateTime dateOnly = new DateTime(1900,01,01,00,00,0);
            var res = (from tr in _contextDCI.TrTraineeData.ToList()
                       join course in content
                       on tr.ScheduleCode equals course.schedule.ScheduleCode.ToString()
                       join emp in _contextDCI.Employees
                       on tr.Empcode.Trim() equals emp.Code.Trim()
                       where emp.Resign == dateOnly
                       select new
                       {
                           course.CourseCode,
                           course.CourseName,
                           course.schedule.ScheduleCode,
                           scheduleStart = new DateTime(course.schedule.ScheduleStart.Value.Year, course.schedule.ScheduleStart.Value.Month, course.schedule.ScheduleStart.Value.Day, course.schedule.ScheduleStart.Value.Hour, course.schedule.ScheduleStart.Value.Minute, course.schedule.ScheduleStart.Value.Second).ToString("dd/MM/yyyy HH:mm"),
                           scheduleEnd = new DateTime(course.schedule.ScheduleEnd.Value.Year, course.schedule.ScheduleEnd.Value.Month, course.schedule.ScheduleEnd.Value.Day, course.schedule.ScheduleEnd.Value.Hour, course.schedule.ScheduleEnd.Value.Minute, course.schedule.ScheduleStart.Value.Second).ToString("dd/MM/yyyy HH:mm"),
                           empcode = tr.Empcode.Trim(),
                           expire = tr.Expire != null ? new DateTime(tr.Expire.Value.Year, tr.Expire.Value.Month, tr.Expire.Value.Day, tr.Expire.Value.Hour, tr.Expire.Value.Minute, course.schedule.ScheduleStart.Value.Second).ToString() :"-",
                           fullname = $"{emp.Name}  {emp.Surn}",
                           trainer = course.schedule.Trainer

                       }).OrderBy(x => x.ScheduleCode).ToList();



            //var content = (from course in _contextDCI.TrCourses.DefaultIfEmpty()
            //                          join trainee in _contextDCI.TrTraineeData
            //                          on course.CourseCode equals trainee.CourseCode
            //                          where course.ExpireStatus == true
            //                          select new
            //                          {
            //                              course.CourseCode,
            //                              course.CourseName,
            //                              course.CourseNameEn,
            //                              trainee.ScheduleCode,
            //                              trainee.Expire,
            //                              empCode = trainee.Empcode.Trim() ?? ""
            //                          }).ToList().GroupBy(x => x.ScheduleCode).ToList();

            //join emp in _contextHRM.Employees
            //on tr equals emp.Code
            //select new
            //{
            //    tr.CourseCode,
            //    tr.CourseName,
            //    tr.CourseNameEn,
            //    tr.empCode,
            //    fullName = $"{emp.Name} {emp.Surn}",
            //    tr.Expire
            //};
            return Ok(res);
        }

        [HttpGet]
        [Route("/brazing/matrix/column")]
        public IActionResult GetColumnMatrix()
        {
            var content = _contextSCM.SkcDictMstrs.Where(x => x.DictType == "LICENSE_LINE_CONTROL").OrderBy(x => x.RefCode).ToList();
            return Ok(content);
        }
        [HttpGet]
        [Route("/brazing/matrix")]
        public IActionResult GetUserOfLicense()
        {
            //var content = _contextSCM.SkcDictMstrs.Where(x => x.DictType == "LICENSE_LINE_CONTROL").OrderBy(x => x.RefCode).ToList();
            var contentCourse = _contextDCI.TrCourses.Where(x => x.ExpireStatus == true).ToList();
            var courseJoinSchedule = (from course in contentCourse
                                      select new
                                      {
                                          course.Id,
                                          course.CourseCode,
                                          course.CourseName,
                                          schedule = _contextDCI.TrSchedules.Where(x => x.Status == "ACTIVE" && x.CourseId == course.Id.ToString()).OrderByDescending(x => x.ScheduleStart).FirstOrDefault()
                                      }).ToList();
            var contentTrainee = (from trainee in _contextDCI.TrTraineeData.ToList()
                                  join course in courseJoinSchedule
                                  on trainee.ScheduleCode equals course.schedule.ScheduleCode.ToString()
                                  where trainee.CourseCode == course.CourseCode
                                  select new
                                  {
                                      empcode = trainee.Empcode.Trim(),
                                      course.CourseCode
                                  }).ToList();
            var users = contentTrainee.GroupBy(x => x.empcode).ToList();
            var courseList = _contextSCM.SkcDictMstrs.Where(x => x.DictType == "LICENSE_LINE_CONTROL").OrderBy(x => x.RefCode).ToList();
            var employee = _contextDCI.Employees.Where(x => x.Resign == DateTime.ParseExact("1900-01-01", "yyyy-MM-dd", null)).ToList();
            List<MUserMatrix> res = new List<MUserMatrix>();
            int i = 1;
            foreach (var user in users)
            {
                var courseOfUser = new Dictionary<string, bool>();
                foreach (var course in courseList)
                {
                    var everTrainee = contentTrainee.FirstOrDefault(x => x.empcode == user.Key && x.CourseCode == course.Code);
                    if (!courseOfUser.ContainsKey(course.Code) && everTrainee != null)
                    {
                        courseOfUser.Add(course.Code, true);
                    }
                }
                var emp = employee.Where(x => x.Code == user.Key).FirstOrDefault();
                MUserMatrix item = new MUserMatrix();
                item.index = i;
                item.empcode = user.Key;
                item.type = (user.Key.Substring(0, 1) == "I" || user.Key.Substring(0, 1) == "i") ? "Subcontract" : "DCI";
                item.course = courseOfUser;
                item.fullName = $"{emp.Name} {emp.Surn}";
                res.Add(item);
                i++;
            }
            return Ok(res);
        }

        [HttpPost]
        [Route("/user/login")]
        public IActionResult Login([FromBody] Employee param)
        {
            var content = _contextDCI.Employees.FirstOrDefault(x => x.Code == param.Code);
            if (content != null)
            {
                return Ok(new
                {
                    status = true,
                    name = $"{content.Name} {content.Surn}",
                    empCode = param.Code,
                });
            }
            else
            {
                return Ok(new
                {
                    status = false
                });
            }
        }
    }
}