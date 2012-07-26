using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using hb.Web.Controllers;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Dynamic;
using System.Collections.ObjectModel;
using hb.Web.Infrastructure;

namespace hb.Web.Infrastructure {
    public class CruddyController:ApplicationController {

        public CruddyController(ITokenHandler tokenStore):base(tokenStore) {}

        protected dynamic _table;
        
        [HttpGet]
        public virtual ActionResult Index(string query) {
            IEnumerable<dynamic> results = null;
            if (!string.IsNullOrEmpty(query)) {
                results = _table.FuzzySearch(query);
            } else {
                results = _table.All(OrderBy: "created_at desc");
            }
            if (Request.IsAjaxRequest()) {
                return hb_to_JSON(results);
            }
            return View(results);
        }
        [HttpGet]
        public virtual ActionResult Details(int id) {
            var result = _table.FindBy(ID: id);
            if (Request.IsAjaxRequest()) {
                return hb_to_JSON(result);
            }
            return View(result);
        }
       // [RequireSecurity]
        [HttpGet]

        public ActionResult Create() {
            return View(_table.Prototype);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        //[RequireSecurity]
        public virtual ActionResult Create(FormCollection collection) {
            var model = _table.CreateFrom(collection);
            try {
                // TODO: Add insert logic here
                _table.Insert(model);
                return RedirectToAction("Index");
            } catch (Exception x) {
                TempData["Error"] = "There was a problem adding this record";
                return View();
            }
        }
       // [RequireSecurity]
        [HttpGet]
        public virtual ActionResult Edit(int id) {
            var model = _table.Get(ID: id);
            model._Table = _table;
            return View(model);
        }

         [HttpPost]
        [ValidateAntiForgeryToken]
       // [RequireSecurity]
        [ValidateInput(false)]

        public virtual ActionResult Edit(int id, FormCollection collection) {
            var model = _table.CreateFrom(collection);
            try {
                // TODO: Add update logic here
                _table.Update(model, id);
                return RedirectToAction("Index");
            } catch (Exception x) {
                TempData["Error"] = "There was a problem editing this record";
                return View(model);
            }
        }

       // [HttpDelete]
      //  [ValidateAntiForgeryToken]
        public virtual ActionResult Delete(int id) {
            try {
                // TODO: Add delete logic here
                _table.Delete(id);
                return RedirectToAction("Index");
            } catch {
                TempData["Error"] = "There was a problem deleting this record";
            }
            return RedirectToAction("Index");
        }
        public ActionResult hb_to_JSON(dynamic content) {
            var serializer = new JavaScriptSerializer();
            serializer.RegisterConverters(new JavaScriptConverter[] { new ExpandoObjectConverter() });
            var json = serializer.Serialize(content);
            Response.ContentType = "application/json";
            return Content(json);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// 
    //public class ExpandoObjectConverter : JavaScriptConverter {
    //    public override IEnumerable<Type> SupportedTypes {
    //        get { return new ReadOnlyCollection<Type>(new List<Type>(new Type[] { typeof(ExpandoObject) })); }
    //    }

    //    public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer) {
    //        ExpandoObject expando = (ExpandoObject)obj;

    //        if (expando != null) {
    //            // Create the representation.
    //            Dictionary<string, object> result = new Dictionary<string, object>();
    //            foreach (KeyValuePair<string, object> item in expando) {
    //                var value = item.Value ?? "";
    //                if (value.GetType() == typeof(DateTime))
    //                    result.Add(item.Key, ((DateTime)value).ToShortDateString());
    //                else
    //                    result.Add(item.Key, value.ToString());
    //            }
    //            return result;
    //        }
    //        return new Dictionary<string, object>();
    //    }
    //    public override object Deserialize(IDictionary<string, object> dictionary, Type type, JavaScriptSerializer serializer) {
    //        return null;
    //    }
    //}

}
