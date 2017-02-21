using System.ComponentModel;

namespace Profile.DAL.Entities
{
    public enum RoleType
    {
        [Description("Роль не задана")]
        None,
        [Description("Администратор")]
        Admin,
        [Description("HR")]
        HR,
        [Description("Ментор")]
        Mentor,
        [Description("Менеджер")]
        Manager,
        [Description("СМ")]
        ScrumMaster,
        [Description("Стажер")]
        Trainee
    }
}
