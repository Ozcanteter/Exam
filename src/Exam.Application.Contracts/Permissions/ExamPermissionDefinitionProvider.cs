using Exam.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace Exam.Permissions;

public class ExamPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(ExamPermissions.GroupName);

        myGroup.AddPermission(ExamPermissions.Dashboard.Host, L("Permission:Dashboard"), MultiTenancySides.Host);
        myGroup.AddPermission(ExamPermissions.Dashboard.Tenant, L("Permission:Dashboard"), MultiTenancySides.Tenant);

        //Define your own permissions here. Example:
        //myGroup.AddPermission(ExamPermissions.MyPermission1, L("Permission:MyPermission1"));

        var challengePermission = myGroup.AddPermission(ExamPermissions.Challenges.Default, L("Permission:Challenges"));
        challengePermission.AddChild(ExamPermissions.Challenges.Create, L("Permission:Create"));
        challengePermission.AddChild(ExamPermissions.Challenges.Edit, L("Permission:Edit"));
        challengePermission.AddChild(ExamPermissions.Challenges.Delete, L("Permission:Delete"));

        var participantPermission = myGroup.AddPermission(ExamPermissions.Participants.Default, L("Permission:Participants"));
        participantPermission.AddChild(ExamPermissions.Participants.Create, L("Permission:Create"));
        participantPermission.AddChild(ExamPermissions.Participants.Edit, L("Permission:Edit"));
        participantPermission.AddChild(ExamPermissions.Participants.Delete, L("Permission:Delete"));

        var progressEntryPermission = myGroup.AddPermission(ExamPermissions.ProgressEntries.Default, L("Permission:ProgressEntries"));
        progressEntryPermission.AddChild(ExamPermissions.ProgressEntries.Create, L("Permission:Create"));
        progressEntryPermission.AddChild(ExamPermissions.ProgressEntries.Edit, L("Permission:Edit"));
        progressEntryPermission.AddChild(ExamPermissions.ProgressEntries.Delete, L("Permission:Delete"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<ExamResource>(name);
    }
}