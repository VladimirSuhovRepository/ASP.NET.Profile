using System.Web.Optimization;

namespace Profile.UI
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/mask.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/tables").Include(
                      "~/Scripts/jquery.dataTables.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/login-page").Include(
                      "~/Scripts/Custom/login.js"));

            bundles.Add(new ScriptBundle("~/bundles/trainees-list-page").Include(
                      "~/Scripts/Custom/traineesList.js"));

            bundles.Add(new ScriptBundle("~/bundles/profile-view-page").Include(
                      "~/Scripts/Custom/traineeProfile.js",
                      "~/Scripts/Custom/toTopPage.js"));

            bundles.Add(new ScriptBundle("~/bundles/profile-edit-page").Include(
                      "~/Scripts/Custom/traineeProfileEdit.js",
                      "~/Scripts/Custom/toTopPage.js"));

            bundles.Add(new ScriptBundle("~/bundles/mentor-profile-page").Include(
                "~/Scripts/Custom/mentorEdit.js"));

            bundles.Add(new ScriptBundle("~/bundles/view-review").Include(
                "~/Scripts/Custom/viewReviews.js"));

            bundles.Add(new ScriptBundle("~/bundles/scrum-review-add").Include(
                "~/Scripts/Custom/scrumAddReview.js"));

            bundles.Add(new ScriptBundle("~/bundles/forgot-password").Include(
                "~/Scripts/Custom/PasswordRecoveryValidation.js"));

            bundles.Add(new ScriptBundle("~/bundles/reset-password").Include(
                "~/Scripts/Custom/PasswordRecoveryNewPass.js"));

            bundles.Add(new ScriptBundle("~/bundles/manager-projects").Include(
                "~/Scripts/datepicker.min.js",
                "~/Scripts/Custom/addingProjectTeam.js",
                "~/Scripts/Custom/addingProjectTeamPopup.js"));

            bundles.Add(new ScriptBundle("~/bundles/manager-roles").Include(
                "~/Scripts/Custom/manager/rightsEdit/editRoles.js",
                "~/Scripts/Custom/manager/rightsEdit/mainRoles.js",
                "~/Scripts/Custom/manager/rightsEdit/setRoles.js"));

            bundles.Add(new ScriptBundle("~/bundles/project-description").Include(
                "~/Scripts/Custom/ProjectDescription.js"));

            // Styles
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/styles.css",
                      "~/Content/datepicker.min.css"));

            bundles.Add(new StyleBundle("~/Content/tables").Include(
                      "~/Content/jquery.dataTables.min.css"));
        }
    }
}
