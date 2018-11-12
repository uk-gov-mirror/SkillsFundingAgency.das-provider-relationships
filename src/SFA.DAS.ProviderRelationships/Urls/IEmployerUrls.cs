
namespace SFA.DAS.ProviderRelationships.Urls
{
    public interface IEmployerUrls
    {
        #region Accounts

        string YourAccounts();
        string NotificationSettings();
        string RenameAccount(string hashedAccountId = null);
        string YourTeam(string hashedAccountId = null);
        string YourOrganisationsAndAgreements(string hashedAccountId = null);
        string PayeSchemes(string hashedAccountId = null);

        #endregion Accounts

        #region Commitments

        string Apprentices(string hashedAccountId = null);

        #endregion Commitments

        #region Finance
        #endregion Finance

        #region Portal

        string Homepage();
        string AccountHomepage(string hashedAccountId = null);
        string FinanceHomepage(string hashedAccountId = null);
        string SignIn();
        string SignOut();
        string Help();
        string Privacy();

        #endregion Portal

        #region Recruit
        #endregion Recruit
    }
}