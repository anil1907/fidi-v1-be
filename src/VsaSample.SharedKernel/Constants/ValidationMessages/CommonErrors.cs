namespace VsaSample.SharedKernel.Constants.ValidationMessages;

public static class CommonErrors
{
    private static string GetMessage(IReadOnlyDictionary<string, string> messages, string language) =>
        messages.TryGetValue(language, out var message) ? message : messages[Cultures.En];

    private static readonly IReadOnlyDictionary<string, string> ErrorOccurredMessages = new Dictionary<string, string>
    {
        [Cultures.En] = "An unexpected error occurred.",
        [Cultures.Tr] = "Beklenmeyen bir hata meydana geldi."
    };

    private static readonly IReadOnlyDictionary<string, string> NotFoundMessages = new Dictionary<string, string>
    {
        [Cultures.En] = "Not found.",
        [Cultures.Tr] = "Bulunamadı."
    };

    private static readonly IReadOnlyDictionary<string, string> InvalidJsonMessages = new Dictionary<string, string>
    {
        [Cultures.En] = "Invalid JSON in request body.",
        [Cultures.Tr] = "İstek gövdesinde geçersiz JSON."
    };

    private static readonly IReadOnlyDictionary<string, string> InvalidQueryMessages = new Dictionary<string, string>
    {
        [Cultures.En] = "Invalid query string.",
        [Cultures.Tr] = "Geçersiz sorgu dizesi."
    };

    private static readonly IReadOnlyDictionary<string, string> UnauthorizedMessages = new Dictionary<string, string>
    {
        [Cultures.En] = "You are not authorized to perform this action.",
        [Cultures.Tr] = "Bu işlemi gerçekleştirmek için yetkiniz yok."
    };

    private static readonly IReadOnlyDictionary<string, string> EntityNotFoundByIdMessages = new Dictionary<string, string>
    {
        [Cultures.En] = "{0} with id '{1}' was not found",
        [Cultures.Tr] = "Id = '{1}' olan {0} bulunamadı"
    };

    private static readonly IReadOnlyDictionary<string, string> EntityNotFoundByNameMessages = new Dictionary<string, string>
    {
        [Cultures.En] = "{0} with given name was not found",
        [Cultures.Tr] = "Belirtilen isme sahip {0} bulunamadı"
    };

    private static readonly IReadOnlyDictionary<string, string> PropNameNotEmptyMessages = new Dictionary<string, string>
    {
        [Cultures.En] = "{0} cannot be empty.",
        [Cultures.Tr] = "{0} boş olamaz."
    };

    private static readonly IReadOnlyDictionary<string, string> PropNameNotNullMessages = new Dictionary<string, string>
    {
        [Cultures.En] = "{0} cannot be null.",
        [Cultures.Tr] = "{0} null olamaz."
    };

    private static readonly IReadOnlyDictionary<string, string> PropNameIsRequiredMessages = new Dictionary<string, string>
    {
        [Cultures.En] = "{0} is required.",
        [Cultures.Tr] = "{0} zorunludur."
    };

    private static readonly IReadOnlyDictionary<string, string> PropNameMaxLengthMessages = new Dictionary<string, string>
    {
        [Cultures.En] = "{0} must be at most {1} characters.",
        [Cultures.Tr] = "{0} en fazla {1} karakter olmalıdır."
    };

    private static readonly IReadOnlyDictionary<string, string> NonNegativeMessages = new Dictionary<string, string>
    {
        [Cultures.En] = "Value must be {0} or greater.",
        [Cultures.Tr] = "Değer {0} veya daha büyük olmalıdır."
    };

    public static string ErrorOccurred(string language = Cultures.En) =>
        GetMessage(ErrorOccurredMessages, language);

    public static string NotFound(string language = Cultures.En) =>
        GetMessage(NotFoundMessages, language);

    public static string InvalidJson(string language = Cultures.En) =>
        GetMessage(InvalidJsonMessages, language);

    public static string InvalidQuery(string language = Cultures.En) =>
        GetMessage(InvalidQueryMessages, language);

    public static string Unauthorized(string language = Cultures.En) =>
        GetMessage(UnauthorizedMessages, language);

    public static string EntityNotFoundById(string entityNameEn, string entityNameTr, long id, string language = Cultures.En)
    {
        var entityName = language == Cultures.Tr ? entityNameTr : entityNameEn;
        return string.Format(GetMessage(EntityNotFoundByIdMessages, language), entityName, id);
    }

    public static string EntityNotFoundByName(string entityNameEn, string entityNameTr, string language = Cultures.En)
    {
        var entityName = language == Cultures.Tr ? entityNameTr : entityNameEn;
        return string.Format(GetMessage(EntityNotFoundByNameMessages, language), entityName);
    }

    public static string PropNameNotEmpty(string propName, string language = Cultures.En) =>
        string.Format(GetMessage(PropNameNotEmptyMessages, language), propName);

    public static string PropNameNotNull(string propName, string language = Cultures.En) =>
        string.Format(GetMessage(PropNameNotNullMessages, language), propName);

    public static string PropNameIsRequired(string propName, string language = Cultures.En) =>
        string.Format(GetMessage(PropNameIsRequiredMessages, language), propName);

    public static string PropNameMaxLength(string propName, int value, string language = Cultures.En) =>
        string.Format(GetMessage(PropNameMaxLengthMessages, language), propName, value);

    public static string NonNegative(int value, string language = Cultures.En) =>
        string.Format(GetMessage(NonNegativeMessages, language), value);
}
