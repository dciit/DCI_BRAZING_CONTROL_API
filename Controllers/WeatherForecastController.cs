using BrazingControlAPI.Contexts;
using BrazingControlAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System;
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
            var res = GetBrazingControl(lineControl);


            //var res = (from tr in _contextDCI.TrTraineeData.ToList().DefaultIfEmpty()
            //           join course in content
            //           on tr.ScheduleCode equals course.schedule.ScheduleCode.ToString()
            //           join emp in _contextDCI.Employees
            //            on tr.Empcode.Trim() equals emp.Code.Trim()
            //           where emp.Resign == dateOnly && tr.ScheduleCode != "" && (tr.Expire >= new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0) || tr.Expire == null)
            //           select new
            //           {
            //               course.CourseCode,
            //               course.CourseName,
            //               course.schedule.ScheduleCode,
            //               scheduleStart = new DateTime(course.schedule.ScheduleStart.Value.Year, course.schedule.ScheduleStart.Value.Month, course.schedule.ScheduleStart.Value.Day, course.schedule.ScheduleStart.Value.Hour, course.schedule.ScheduleStart.Value.Minute, course.schedule.ScheduleStart.Value.Second).ToString("dd/MM/yyyy HH:mm"),
            //               scheduleEnd = new DateTime(course.schedule.ScheduleEnd.Value.Year, course.schedule.ScheduleEnd.Value.Month, course.schedule.ScheduleEnd.Value.Day, course.schedule.ScheduleEnd.Value.Hour, course.schedule.ScheduleEnd.Value.Minute, course.schedule.ScheduleStart.Value.Second).ToString("dd/MM/yyyy HH:mm"),
            //               empcode = tr.Empcode.Trim(),
            //               expire = tr.Expire != null ? new DateTime(tr.Expire.Value.Year, tr.Expire.Value.Month, tr.Expire.Value.Day, tr.Expire.Value.Hour, tr.Expire.Value.Minute, course.schedule.ScheduleStart.Value.Second).ToString() : "-",
            //               fullname = $"{emp.Name}  {emp.Surn}",
            //               trainer = course.schedule.Trainer

            //           }).OrderBy(x => x.ScheduleCode).ToList();
            return Ok(res);
        }

        private object GetBrazingControl(string lineControl = "")
        {
            var content = (from course in _contextDCI.TrCourses
                           where course.ExpireStatus == true
                           select new
                           {
                               course.Id,
                               course.CourseCode,
                               course.CourseName
                           }).ToList();
            if (lineControl != "" && lineControl != "-")
            {
                content = content.Where(x => x.CourseCode == lineControl).ToList();
            }
            DateTime dateOnly = new DateTime(1900, 01, 01, 00, 00, 0);
            DateTime dt = DateTime.Now;

            var EmpIsNull = _contextDCI.TrTraineeData.Where(x => x.Expire == null && x.CourseCode != "" && x.ScheduleCode != "").ToList();
            foreach (var ItemEmp in EmpIsNull)
            {
                var dateTraining = _contextDCI.TrSchedules.FirstOrDefault(x => x.ScheduleCode.ToString() == ItemEmp.ScheduleCode);
                if (dateTraining != null)
                {
                    var cloneItemExp = ItemEmp;
                    cloneItemExp.Expire = dateTraining.ScheduleStart.Value.AddYears(1);
                    _contextDCI.TrTraineeData.Update(cloneItemExp);
                }
            }
            int update = _contextDCI.SaveChanges();

            var emp = (from employee in _contextDCI.Employees.Where(x => x.Resign == dateOnly).ToList()
                       select new
                       {
                           employee.Code,
                           employee.Resign,
                           fullname = employee.Name + "." + employee.Surn.Substring(0, 1)
                       }).ToList();
            var res = (
                from course in content.DefaultIfEmpty()
                join schedule in _contextDCI.TrSchedules.Where(s => s.Status == "ACTIVE" && s.ScheduleEnd.Value.AddYears(1) >= dt).ToList()
                on course.Id.ToString() equals schedule.CourseId
                join tr in _contextDCI.TrTraineeData.Where(e => e.CourseCode != null && e.ScheduleCode != null && e.Expire != null).ToList()
                on schedule.ScheduleCode.ToString() equals tr.ScheduleCode
                where tr.ScheduleCode != "" && (tr.Expire >= new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0) || tr.Expire == null)
                select new
                {
                    course.CourseCode,
                    course.CourseName,
                    schedule.ScheduleCode,
                    ScheduleEnd = schedule.ScheduleEnd.Value.AddYears(1),
                    tr.Empcode,
                    fullname = (emp.FirstOrDefault(x => x.Code == tr.Empcode.Trim()) != null ? emp.FirstOrDefault(x => x.Code == tr.Empcode.Trim()).fullname : "-"),
                    trainer = (schedule.Trainer != "" && schedule.Trainer != null) ? schedule.Trainer : "-",
                    expire = new DateTime(tr.Expire.Value.Year, tr.Expire.Value.Month, tr.Expire.Value.Day, tr.Expire.Value.Hour, tr.Expire.Value.Minute, 0).ToString(),
                    scheduleStart = new DateTime(schedule.ScheduleStart.Value.Year, schedule.ScheduleStart.Value.Month, schedule.ScheduleStart.Value.Day, schedule.ScheduleStart.Value.Hour, schedule.ScheduleStart.Value.Minute, schedule.ScheduleStart.Value.Second).ToString("dd/MM/yyyy HH:mm")
                }
                ).Where(x => x.fullname != "-").OrderBy(x => x.ScheduleCode).ToList();
            return res;
        }

        [HttpGet]
        [Route("/brazing/matrix/column")]
        public IActionResult GetColumnMatrix()
        {
            var content = _contextSCM.SkcDictMstrs.Where(x => x.DictType == "LICENSE_LINE_CONTROL").Select(x => new
            {
                x.DictId,
                x.Code,
                x.CreateDate,
                x.DictType,
                x.RefItem,
                DictDesc = Convert.ToInt32(x.DictDesc),
                x.UpdateDate,
                x.Note,
                x.DictStatus,
            }).OrderBy(x => x.DictDesc).ToList();
            return Ok(content);
        }
        [HttpGet]
        [Route("/brazing/matrix")]
        public IActionResult GetUserOfLicense()
        {
            //var content = _contextSCM.SkcDictMstrs.Where(x => x.DictType == "LICENSE_LINE_CONTROL").OrderBy(x => x.RefCode).ToList();
            //var contentCourse = _contextDCI.TrCourses.Where(x => x.ExpireStatus == true).ToList();
            //var courseJoinSchedule = (from course in contentCourse.DefaultIfEmpty()
            //                          select new
            //                          {
            //                              course.Id,
            //                              course.CourseCode,
            //                              course.CourseName,
            //                              schedule = _contextDCI.TrSchedules.Where(x => x.Status == "ACTIVE" && x.CourseId == course.Id.ToString()).OrderByDescending(x => x.ScheduleStart).FirstOrDefault()
            //                          }).ToList();



            //DateTime dateOnly = new DateTime(1900, 01, 01, 00, 00, 0);
            //DateTime dt = DateTime.Now;


            //var contentTrainee = (from tr in _contextDCI.TrTraineeData.ToList()
            //                      join course in contentCourse
            //                      on tr.ScheduleCode equals course.CourseCode
            //                      where tr.CourseCode == course.CourseCode
            //                      select new
            //                      {
            //                          empcode = tr.Empcode.Trim(),
            //                          course.CourseCode
            //                      }).ToList();

            //var users = contentTrainee.GroupBy(x => x.empcode).ToList();

            //var employee = _contextDCI.Employees.Where(x => x.Resign == DateTime.ParseExact("1900-01-01", "yyyy-MM-dd", null)).ToList();
            var content = (from course in _contextDCI.TrCourses
                           where course.ExpireStatus == true
                           select new
                           {
                               course.Id,
                               course.CourseCode,
                               course.CourseName
                           }).ToList();
            DateTime dateOnly = new DateTime(1900, 01, 01, 00, 00, 0);
            DateTime dt = DateTime.Now;

            var EmpIsNull = _contextDCI.TrTraineeData.Where(x => x.Expire == null && x.CourseCode != "" && x.ScheduleCode != "").ToList();
            foreach (var ItemEmp in EmpIsNull)
            {
                var dateTraining = _contextDCI.TrSchedules.FirstOrDefault(x => x.ScheduleCode.ToString() == ItemEmp.ScheduleCode);
                if (dateTraining != null)
                {
                    var cloneItemExp = ItemEmp;
                    cloneItemExp.Expire = dateTraining.ScheduleStart.Value.AddYears(1);
                    _contextDCI.TrTraineeData.Update(cloneItemExp);
                }
            }
            int update = _contextDCI.SaveChanges();

            var emp = (from employee in _contextDCI.Employees.Where(x => x.Resign == dateOnly).ToList()
                       select new
                       {
                           employee.Code,
                           employee.Resign,
                           fullname = employee.Name + "." + employee.Surn.Substring(0, 1)
                       }).ToList();
            var data = (
                from course in content.DefaultIfEmpty()
                join schedule in _contextDCI.TrSchedules.Where(s => s.Status == "ACTIVE" && s.ScheduleEnd.Value.AddYears(1) >= dt).ToList()
                on course.Id.ToString() equals schedule.CourseId
                join tr in _contextDCI.TrTraineeData.Where(e => e.CourseCode != null && e.ScheduleCode != null && e.Expire != null).ToList()
                on schedule.ScheduleCode.ToString() equals tr.ScheduleCode
                where tr.ScheduleCode != "" && (tr.Expire >= new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0) || tr.Expire == null)
                select new
                {
                    course.CourseCode,
                    course.CourseName,
                    schedule.ScheduleCode,
                    ScheduleEnd = schedule.ScheduleEnd.Value.AddYears(1),
                    tr.Empcode,
                    fullname = (emp.FirstOrDefault(x => x.Code == tr.Empcode.Trim()) != null ? emp.FirstOrDefault(x => x.Code == tr.Empcode.Trim()).fullname : "-"),
                    trainer = (schedule.Trainer != "" && schedule.Trainer != null) ? schedule.Trainer : "-",
                    expire = new DateTime(tr.Expire.Value.Year, tr.Expire.Value.Month, tr.Expire.Value.Day, tr.Expire.Value.Hour, tr.Expire.Value.Minute, 0).ToString(),
                    scheduleStart = new DateTime(schedule.ScheduleStart.Value.Year, schedule.ScheduleStart.Value.Month, schedule.ScheduleStart.Value.Day, schedule.ScheduleStart.Value.Hour, schedule.ScheduleStart.Value.Minute, schedule.ScheduleStart.Value.Second).ToString("dd/MM/yyyy HH:mm")
                }
                ).Where(x => x.fullname != "-").OrderBy(x => x.ScheduleCode).ToList();
            List<MUserMatrix> res = new List<MUserMatrix>();
            int i = 1;
            var gr = data.GroupBy(x => x.Empcode);
            var courseList = _contextSCM.SkcDictMstrs.Where(x => x.DictType == "LICENSE_LINE_CONTROL").OrderBy(x => x.RefCode).ToList();
            foreach (var user in gr)
            {
                var courseOfUser = new Dictionary<string, bool>();
                foreach (var course in courseList)
                {
                    var everTrainee = data.FirstOrDefault(x => x.Empcode == user.Key && x.CourseCode == course.Code);
                    if (!courseOfUser.ContainsKey(course.Code) && everTrainee != null)
                    {
                        courseOfUser.Add(course.Code, true);
                    }
                }
                var tr = emp.Where(x => x.Code == user.Key.Trim()).FirstOrDefault();
                MUserMatrix item = new MUserMatrix();
                item.index = i;
                item.empcode = user.Key;
                item.type = (user.Key.Substring(0, 1) == "I" || user.Key.Substring(0, 1) == "i") ? "Subcontract" : "DCI";
                item.course = courseOfUser;
                item.fullName = $"{tr.fullname}";
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