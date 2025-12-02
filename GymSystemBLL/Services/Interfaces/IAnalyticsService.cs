using GymSystemBLL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemBLL.Services.Interfaces
{
    public interface IAnalyticsService
    {
        // Get Analytics Data

        AnalyticsViewModel GetAnalyticsData();
    }
}
