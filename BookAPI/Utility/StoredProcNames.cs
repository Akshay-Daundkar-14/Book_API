namespace BookAPI.Utility
{
    public static class StoredProcNames
    {
        public static string AddNewBook{ get { return "AddBook"; }  }
        public static string UpdateExistingBook{ get { return "UpdateBook"; }  }
        public static string GetAllBooks{ get { return "GetBooks"; }  }
        public static string DeleteExistingBook{ get { return "DeleteBook"; }  }
    }
}
