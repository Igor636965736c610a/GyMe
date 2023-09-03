namespace GymAppCore.Models.Entities.Configurations;

public static class EntitiesConfig
{
    public static class UserConf
    {
        public const int FirstNameMaxLength = 20;
        public const int FirstNameMinLenght = 2;
        public const int LastNameLength = 20;
        public const int LastNameMinLenght = 2;
        public const int UserNameMaxLength = 30;
        public const int UserNameMinLenght = 2;
        public const int AccountProviderMaxLength = 30;
    }
    public static class ExtendedUserConf
    {
        public const int DescriptionMaxLenght = 400;
    }
    public static class SimpleExerciseConf
    {
        public const int DescriptionMaxLength = 200;
    }

    public static class SeriesConf
    {
        public const int WeightMaxLenght = 100000;
        public const int WeightMinLenght = 1;
        public const int NumberOfRepetitionsMaxLenght = 1000;
        public const int NumberOfRepetitionsMinLenght = 1;
    }
}