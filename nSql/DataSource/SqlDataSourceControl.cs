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
    [PersistChildren(false), ParseChildren(true)]
    //[Designer(typeof(CustomDataSourceDesigner))]
    public class SqlDataSourceControl : DataSourceControl
    {
        #region Fields

		protected static readonly string[] _views = { "DefaultView" };

		protected SqlDataSourceView _view;

		#endregion

		#region Properties

		[Category("Data"), DefaultValue("")]
		public string TypeName
		{
			get { return View.TypeName; }
			set { View.TypeName = value; }
		}

		[Category("Data"), DefaultValue("")]
		public string SelectMethod
		{
			get { return View.SelectMethod; }
			set { View.SelectMethod = value; }
		}

		[PersistenceMode(PersistenceMode.InnerProperty), Category("Data"), DefaultValue((string) null), MergableProperty(false), Editor(typeof(ParameterCollectionEditor), typeof(UITypeEditor))]
		public ParameterCollection SelectParameters
		{
			get { return View.SelectParameters;	}
		}

		protected SqlDataSourceView View
		{
			get {
				if (_view == null){
					_view = new SqlDataSourceView(this, _views[0]);
					if (base.IsTrackingViewState) {
						((IStateManager)_view).TrackViewState();
					}
				}
				return _view;
			}
		}

		#endregion

		#region Methods

		#region Constructors

		public SqlDataSourceControl()
		{
		}

		#endregion

		#region IDataSource Methods

		protected override DataSourceView GetView(string viewName)
		{
			if ((viewName == null) || ((viewName.Length != 0) && (String.Compare(viewName, "DefaultView", StringComparison.OrdinalIgnoreCase) != 0))){
				throw new ArgumentException("An invalid view was requested", "viewName");
			}

			return View;
		}

		protected override ICollection GetViewNames()
		{
			return _views;
		}

		#endregion

		#region ViewState Management

		// We use a pair to store the view state. The first element is used to store the parent's view state
		// The second element is used to store the view's view state

		protected override void LoadViewState(object savedState)
		{
			Pair previousState = (Pair) savedState;

			if (savedState == null) {
				base.LoadViewState(null);
			} else {
				base.LoadViewState(previousState.First);

				if (previousState.Second != null) {
					((IStateManager) View).LoadViewState(previousState.Second);
				}
			}
		}

		protected override object SaveViewState()
		{
			Pair currentState = new Pair();

			currentState.First = base.SaveViewState();

			if (_view != null) {
				currentState.Second = ((IStateManager) View).SaveViewState();
			}

			if ((currentState.First == null) && (currentState.Second == null)) {
				return null;
			}

			return currentState;
		}

		protected override void TrackViewState()
		{
			base.TrackViewState();

			if (_view != null) {
				((IStateManager) View).TrackViewState();
			}
		}

		#endregion

		#region Life Cycle Related Methods

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);

			// handle the LoadComplete event to update select parameters
			if (Page != null) {
				Page.LoadComplete += new EventHandler(UpdateParameterValues);
			}
		}

		protected virtual void UpdateParameterValues(object sender, EventArgs e)
		{
			SelectParameters.UpdateValues(Context, this);
		}

		#endregion

		#region Select Method

		public IEnumerable Select()
		{
			return View.Select(DataSourceSelectArguments.Empty);
		}

		#endregion

		#endregion
    }
}
*/