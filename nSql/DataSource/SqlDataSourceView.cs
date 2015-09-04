#region using directives
using System;
#region System.Collections
    using System.Collections;
    using System.Collections.Generic;
#endregion
using System.Text;
using System.Drawing.Design;
#region System.Web
    using System.Web.UI;
    using System.Web.UI.Design.WebControls;
    using System.Web.UI.WebControls;
#endregion
using System.ComponentModel;
#endregion

/*
namespace nTools.SqlTools
{
    public class SqlDataSourceView : DataSourceView, IStateManager
    {
        public enum dbType
        {
            MsSql = 1,
            MySql = 2,
        }

    #region Fields
        private dbType _viewType;
        private bool _tracking;
        protected SqlDataSourceControl _owner;  //if this whole gig works, will have 2 owners...one for ms, one for my
        protected string _typeName;
        protected string _selectMethod;
        protected ParameterCollection _selectParameters;
    #endregion

    #region Properties
    
        #region DataSourceView's CanXXX Properties

        public override bool CanInsert
        {
            get { return false; }
        }

        public override bool CanUpdate
        {
            get { return false; }
        }

        public override bool CanDelete
        {
            get { return false; }
        }

        public override bool CanRetrieveTotalRowCount
        {
            get { return false; }
        }

        public override bool CanSort
        {
            get { return false; }
        }

        public override bool CanPage
        {
            get { return false; }
        }

        #endregion

        #region Other Properties

        public string TypeName
        {
            get
            {
                if (_typeName == null)
                {
                    return String.Empty;
                }
                return _typeName;
            }
            set
            {
                if (TypeName != value)
                {
                    _typeName = value;
                    OnDataSourceViewChanged(EventArgs.Empty);
                }
            }
        }

        public string SelectMethod
        {
            get
            {
                if (_selectMethod == null)
                {
                    return String.Empty;
                }
                return _selectMethod;
            }
            set
            {
                if (SelectMethod != value)
                {
                    _selectMethod = value;
                    OnDataSourceViewChanged(EventArgs.Empty);
                }
            }
        }

        public ParameterCollection SelectParameters
        {
            get
            {
                if (_selectParameters == null)
                {
                    _selectParameters = new ParameterCollection();
                    _selectParameters.ParametersChanged += new EventHandler(ParametersChangedEventHandler);
                    if (_tracking)
                    {
                        ((IStateManager)_selectParameters).TrackViewState();
                    }
                }
                return _selectParameters;
            }
        }

        #endregion

    #endregion

    #region Methods

        #region cstr
        public SqlDataSourceView(SqlDataSourceControl owner, string name) : base(owner, name)
        {
            _owner = owner;
            _viewType = dbType.MsSql;
        }
        #endregion

        #region IStateManager Methods

        bool IStateManager.IsTrackingViewState
        {
            get { return _tracking; }
        }

        void IStateManager.LoadViewState(object savedState)
        {
            LoadViewState(savedState);
        }

        object IStateManager.SaveViewState()
        {
            return SaveViewState();
        }

        void IStateManager.TrackViewState()
        {
            TrackViewState();
        }

        protected virtual void LoadViewState(object savedState)
        {
            if (savedState != null)
            {
                if (savedState != null)
                {
                    ((IStateManager)SelectParameters).LoadViewState(savedState);
                }
            }
        }

        protected virtual object SaveViewState()
        {
            if (_selectParameters != null)
            {
                return ((IStateManager)_selectParameters).SaveViewState();
            }
            else
            {
                return null;
            }
        }

        protected virtual void TrackViewState()
        {
            _tracking = true;

            if (_selectParameters != null)
            {
                ((IStateManager)_selectParameters).TrackViewState();
            }
        }

        #endregion

        #region ExecuteXXX Methods

        protected override IEnumerable ExecuteSelect(DataSourceSelectArguments arguments)
        {
            // if there isn't a select method, error
            if (SelectMethod.Length == 0)
            {
                throw new InvalidOperationException(_owner.ID + ": There isn't a SelectMethod defined");
            }

            // check if we support the capabilities the data bound control expects
            arguments.RaiseUnsupportedCapabilitiesError(this);

            // gets the select parameters and their values
            IOrderedDictionary selParams = SelectParameters.GetValues(System.Web.HttpContext.Current, _owner);

            // gets the data mapper
            Type type = BuildManager.GetType(_typeName, false, true);

            if (type == null)
            {
                throw new NotSupportedException(_owner.ID + ": TypeName not found!");
            }

            // gets the method to call
            MethodInfo method = type.GetMethod(SelectMethod, BindingFlags.Public | BindingFlags.Static);

            if (method == null)
            {
                throw new InvalidOperationException(_owner.ID + ": SelectMethod not found!");
            }

            // creates a dictionary with the parameters to call the method
            ParameterInfo[] parameters = method.GetParameters();
            IOrderedDictionary paramsAndValues = new OrderedDictionary(parameters.Length);

            // check that all parameters that the method needs are in the SelectParameters
            foreach (ParameterInfo currentParam in parameters)
            {
                string paramName = currentParam.Name;

                if (!selParams.Contains(paramName))
                {
                    throw new InvalidOperationException(_owner.ID + ": The SelectMethod doesn't have a parameter for " + paramName);
                }
            }

            // save the parameters and its values into a dictionary
            foreach (ParameterInfo currentParam in parameters)
            {
                string paramName = currentParam.Name;
                object paramValue = selParams[paramName];

                if (paramValue != null)
                {
                    // check if we have to convert the value
                    // if we have a string value that needs conversion
                    if (!currentParam.ParameterType.IsInstanceOfType(paramValue) && (paramValue is string))
                    {

                        // try to get a type converter
                        TypeConverter converter = TypeDescriptor.GetConverter(currentParam.ParameterType);
                        if (converter != null)
                        {
                            try
                            {
                                // try to convert the string using the type converter
                                paramValue = converter.ConvertFromString(null, System.Globalization.CultureInfo.CurrentCulture, (string)paramValue);
                            }
                            catch (Exception)
                            {
                                throw new InvalidOperationException(_owner.ID + ": Can't convert " + paramName + " from string to " + currentParam.ParameterType.Name);
                            }
                        }
                    }
                }

                paramsAndValues.Add(paramName, paramValue);
            }

            object[] paramValues = null;

            // if the method has parameters, create an array to store parameters values
            if (paramsAndValues.Count > 0)
            {
                paramValues = new object[paramsAndValues.Count];
                for (int i = 0; i < paramsAndValues.Count; i++)
                {
                    paramValues[i] = paramsAndValues[i];
                }
            }

            object returnValue = null;

            try
            {
                // call the method
                returnValue = method.Invoke(null, paramValues);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(_owner.ID + ": Error calling the SelectMethod", e);
            }

            return (IEnumerable)returnValue;
        }

        protected override int ExecuteInsert(IDictionary values)
        {
            throw new NotImplementedException();
        }

        protected override int ExecuteUpdate(IDictionary keys, IDictionary values, IDictionary oldValues)
        {
            throw new NotImplementedException();
        }

        protected override int ExecuteDelete(IDictionary keys, IDictionary oldValues)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Helpers

        public IEnumerable Select(DataSourceSelectArguments args)
        {
            return ExecuteSelect(args);
        }

        protected void ParametersChangedEventHandler(object o, EventArgs e)
        {
            OnDataSourceViewChanged(EventArgs.Empty);
        }

        #endregion

    #endregion

    }//end class

}//end namespace
*/