using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.ControllerModels;
using ViewModels.DataModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EOMobile
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class WorkOrderReportPage : ContentPage
	{
        public List<WorkOrderInventoryDTO> workOrderList;
		public WorkOrderReportPage ()
		{
			InitializeComponent ();

        }

        public void OnShowReportsClicked(object sender, EventArgs e)
        {
            WorkOrderListFilter filter = new WorkOrderListFilter();
            filter.FromDate = this.WorkOrderFromDate.Date;
            filter.ToDate = this.WorkOrderToDate.Date;

            workOrderList = ((App)App.Current).GetWorkOrders(filter);
        }
    }
}