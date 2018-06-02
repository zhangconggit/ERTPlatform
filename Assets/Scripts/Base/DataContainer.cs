using System;
using System.Collections.Generic;

public class DataContainer
{
    private static DataContainer _instance = null;
    public static DataContainer Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new DataContainer();
            }
            return _instance;
        }
    }

    public string getStepData(string key,string _modelname)
    {

        return "" ;
    }
}

