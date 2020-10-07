using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mihaylov.Dictionaries.Core.Interfaces;
using Mihaylov.Dictionaries.Data.Models;
using Mihaylov.Web.Common.Toastr;
using Mihaylov.Web.Controllers.Base;
using Mihaylov.Web.ViewModels.Dictionaries;

namespace Mihaylov.Web.Controllers
{
    public class DictionariesController : BaseController
    {
        private const string NAME = "Dictionaries";

        private readonly ICoursesInfoManager coursesManager;
        private readonly IRecordsManager recordsManager;
        private readonly IRecordsWriter recordsWriter;

        public DictionariesController(ICoursesInfoManager coursesManager, IRecordsManager recordsManager,
            IRecordsWriter recordsWriter, ILoggerFactory logger, IToastrHelper toaster)
            : base(logger, toaster)
        {
            this.coursesManager = coursesManager;
            this.recordsManager = recordsManager;
            this.recordsWriter = recordsWriter;
        }

        public IActionResult Index(int? courseId)
        {
            if (!courseId.HasValue)
            {
                return this.RedirectToAction(nameof(this.ChooseCourse));
            }

            var model = new IndexExtendedViewModel()
            {
                Record = new RecordViewModel(courseId.Value, this.recordsManager.GetAllRecordTypes()),
                Course = this.coursesManager.GetCourseById(courseId.Value),
            };

            return this.View(model);
        }

        [HttpPost]
        public ActionResult Save(RecordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                if (model.CourseId > 0)
                {
                    var indexViewModel = new IndexExtendedViewModel()
                    {
                        Record = model,
                        Course = this.coursesManager.GetCourseById(model.CourseId),
                    };

                    return this.View(nameof(this.Index), indexViewModel);
                }
                else
                {
                    return this.RedirectToAction(nameof(this.ChooseCourse));
                }
            }

            ICollection<RecordType> recordsType = model.RecordTypes.Where(r => r.Selected)
                                                                   .Select(r => new RecordType(r.Id, r.Name))
                                                                   .ToList();

            Record record = new Record(model.Id, model.CourseId, model.ModuleNumber, model.Original, model.Translation,
                model.Comment, recordsType);

            this.recordsWriter.AddRecord(record);
            this.AddToastMessage(string.Empty, "Record saved!", ToastType.Success);

            return this.RedirectToAction(nameof(this.Index), new { courseId = model.CourseId });
        }

        [HttpGet]
        public ActionResult ChooseCourse()
        {
            var model = new ChooseCourseExtendedModel()
            {
                Courses = this.coursesManager.GetAllCourses().OrderByDescending(c => c.StartDate),
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult ChooseCourse(ChooseCourseModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.RedirectToAction(nameof(this.ChooseCourse));
            }

            return this.RedirectToAction(nameof(this.Index), new { courseId = model.CourseId });
        }
    }
}