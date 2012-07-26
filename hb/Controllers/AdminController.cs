using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Web.Script.Serialization;

using System.Dynamic;
using hb.Domain.Models;
using System.Collections.ObjectModel;
using hb.Web.Infrastructure;
using hb.Web.Controllers;

namespace hb.WebUI.Controllers
{
   [RequireSecurity]
    public class AdminController : ApplicationController
    {

        protected dynamic _table = new Posts();

         public AdminController(ITokenHandler tokenStore):base(tokenStore) {
            
           
        }


        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SavePost()
        {

            var model = SqueezeJson();

            if (model.id == "0")
            {
               

               dynamic r = new ExpandoObject();
                r.summary = model.summary;
                r.body = model.body;
                r.title = model.title;

                _table.Insert(r);

                return hb_to_JSON(model); ;
            }

            //update post
             _table.Update(model, model.id);
          


            return hb_to_JSON(model);
        }


       
        [HttpGet]
        public ActionResult GetPostDetail(string id)
        {

            var result = _table.FindBy(ID: id);
            return hb_to_JSON(result);

          
        }


        [HttpGet]
        public ActionResult GetPosts()
        {

            return hb_to_JSON(_table.All(OrderBy: "created_at desc"));
        }


        [HttpPost]
        public ActionResult DeletePost()
        {
            var model = SqueezeJson();
              _table.Delete(model.id);

            return hb_to_JSON(model);
        }
      

    }//cls
}//ns
