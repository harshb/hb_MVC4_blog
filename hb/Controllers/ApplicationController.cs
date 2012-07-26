﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using hb.Web.Infrastructure;

using System.Web.Script.Serialization;
using System.IO;
using System.Dynamic;
using hb.Domain.Models;

namespace hb.Web.Controllers
{
    public class ApplicationController :hbController
    {
        public ITokenHandler TokenStore;

        public ApplicationController()
        {
            //TokenStore = new FormsAuthTokenStore();
            ////initialize this
            //ViewBag.CurrentUser = CurrentUser ?? new { Email = "" };

        }
        public ApplicationController(ITokenHandler tokenStore)
        {
            TokenStore = tokenStore;

            //initialize this
            ViewBag.CurrentUser = CurrentUser ?? new { Email = "" };
        }

        public bool IsLoggedIn
        {
            get
            {

                return CurrentUser != null;
            }
        }

        dynamic _currentUser;
        public dynamic CurrentUser
        {
            get
            {
                var token = TokenStore.GetToken();
                if (!String.IsNullOrEmpty(token))
                {
                    _currentUser = Users.FindByToken(token);

                    if (_currentUser == null)
                    {
                        //another user logged in
                        //force the current user to be logged out...
                        TokenStore.RemoveClientAccess();
                    }
                }

                //Hip to be null...
                return _currentUser;
            }

        }//
      
        //public ActionResult hb_to_Jason(dynamic content)
        //{
        //    var serializer = new JavaScriptSerializer();
        //    serializer.RegisterConverters(new JavaScriptConverter[] { new ExpandoObjectConverter() });
        //    var json = serializer.Serialize(content);
        //    Response.ContentType = "application/json";
        //    return Content(json);
        //}

        ////this feels hacky
        ////if we dont use this we will have to write models to cast each JSON request
        //public dynamic SqueezeJson()
        //{
        //    var bodyText = "";
        //    using (var stream = Request.InputStream)
        //    {
        //        stream.Seek(0, SeekOrigin.Begin);
        //        using (var reader = new StreamReader(stream))
        //            bodyText = reader.ReadToEnd();
        //    }
        //    if (string.IsNullOrEmpty(bodyText)) return null;
        //    var serializer = new JavaScriptSerializer();
        //    serializer.RegisterConverters(new JavaScriptConverter[] { new ExpandoObjectConverter() });

        //    return serializer.Deserialize(bodyText, typeof(ExpandoObject));
        //}
    
    }//cls
}//ns
