using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace ITSUPPORTTICKETSYSTEM
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection Bundles)
        {
            Bundles.Add(new StyleBundle("~/admin/styles")
                .Include("~/content/styles/new_bootstrap.css")
                  .Include("~/content/styles/admin.css")
                  
                  //.Include("~/content/styles/custom.css")
                  .Include("~/content/styles/font-awesome.css")

                   .Include("~/content/styles/summernote.css"));

            Bundles.Add(new StyleBundle("~/styles")
                .Include("~/content/styles/new_bootstrap.css")
                .Include("~/content/styles/admin.css")
                .Include("~/content/styles/site.css")
                .Include("~/content/styles/font-awesome.css")
                .Include("~/content/styles/jquery-ui-1.10.3.custom.min.css")
                .Include("~/content/styles/entypo.css")
                .Include("~/content/styles/neon-core.css")
                .Include("~/content/styles/neon-theme.css")
                .Include("~/content/styles/neon-forms.css")
                .Include("~/content/styles/fonts.googleapis.css"));
               
            Bundles.Add(new StyleBundle("~/front/styles")
          
             .Include("~/content/styles/theme.css")
              .Include("~/content/styles/fonts.googleapis.css")
                .Include("~/content/styles/portal_utils.css")
           );

            //Bundles.Add(new StyleBundle("~/front/scripts")
            //    .Include("~/scripts/portal_head.js")
            //    .Include("~/scripts/portal_bottom.js")
            //    .Include("~/scripts/redactor.js")
            //    .Include("~/scripts/syntaxhighlighter.js")
            //    .Include("~/scripts/en.js")
            //    .Include("~/scripts/freshfone_portal.js")
            //    );


            Bundles.Add(new ScriptBundle("~/admin/scripts")
                .Include("~/scripts/bootstrap.js")
                .Include("~/scripts/jquery-2.1.4.js")
                .Include("~/scripts/jquery.validate.js")
                .Include("~/scripts/jquery.validate.unobtrusive.js")
                .Include("~/areas/admin/scripts/form.js")
                .Include("~/areas/admin/scripts/jquery.timeago.js")
                .Include("~/areas/admin/scripts/navigation.js")
                .Include("~/areas/admin/scripts/bootstrap-progressbar.min.js")
                .Include("~/areas/admin/scripts/jquery.nicescroll.min.js")
                .Include("~/areas/admin/scripts/custom.js")
                .Include("~/areas/admin/scripts/easypiechart.min.js")
                .Include("~/areas/admin/scripts/king-chart-stat.js"));

            Bundles.Add(new ScriptBundle("~/admin/ticket/scripts")
                .Include("~/areas/admin/scripts/ticketeditor.js")
                 .Include("~/areas/admin/scripts/jquery.timeago.js")
                .Include("~/areas/admin/scripts/timeago.js")
                .Include("~/areas/admin/scripts/commenteditor.js")
                .Include("~/areas/admin/scripts/priorityeditor.js")
                .Include("~/Scripts/summernote/dist/summernote.js"));
             

            Bundles.Add(new ScriptBundle("~/scripts")
              .Include("~/scripts/portal_head.js")
                .Include("~/scripts/portal_bottom.js")
                .Include("~/scripts/redactor.js")
                .Include("~/scripts/syntaxhighlighter.js")
                .Include("~/scripts/en.js")
                .Include("~/scripts/freshfone_portal.js")
             .Include("~/scripts/TweenMax.min.js")
             .Include("~/scripts/jquery-ui-1.10.3.minimal.min.js")
             .Include("~/scripts/jquery-2.1.4.js")
             .Include("~/scripts/jquery.validate.js")
             .Include("~/scripts/jquery.validate.unobtrusive.js")
             .Include("~/scripts/bootstrap.js")
             .Include("~/scripts/joinable.js")
             .Include("~/scripts/resizeable.js")
             .Include("~/scripts/neon-api.js")
             .Include("~/scripts/cookies.min.js")
             .Include("~/scripts/neon-login.js")
             .Include("~/scripts/neon-custom.js")
             .Include("~/scripts/neon-demo.js")
             .Include("~/scripts/neon-skins.js"));
                
                
        }
    }
}