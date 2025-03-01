namespace DoubleDCore.Storage.Base
{
    public static class SaveControllerExtension
    {
        public static void Save(this ISaveController saveController, ISaveObject saveObject)
        {
            saveController.Save(saveObject.Key);
        }

        public static void Load(this ISaveController saveController, ISaveObject saveObject)
        {
            saveController.Load(saveObject.Key);
        }

        public static void Unsubscribe(this ISaveController saveController, ISaveObject saveObject)
        {
            saveController.Unsubscribe(saveObject.Key);
        }
    }
}