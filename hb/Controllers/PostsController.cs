using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using hb.Web.Controllers;
using Massive;
using hb.Web.Infrastructure;
using hb.Domain.Models;

namespace hb.WebUI.Controllers
{
    public class PostsController : CruddyController 
    {

        public PostsController(ITokenHandler tokenStore):base(tokenStore) {
            _table = new Posts();
            ViewBag.Table = _table;
        }

        public ActionResult PostFeed()
        {
            return View(_table.All(OrderBy: "created_at desc"));
        }

        public ActionResult About()
        {
            return View() ;
        }
    }//cls
}//ns
