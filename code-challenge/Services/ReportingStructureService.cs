using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using challenge.Models;
using Microsoft.Extensions.Logging;
using challenge.Repositories;

namespace challenge.Services
{
    public class ReportingStructureService : IReportingStructureService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<ReportingStructureService> _logger;

        public ReportingStructureService(ILogger<ReportingStructureService> logger, IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        // Fill out the ReportingStructure for the employee specified by ID
        public ReportingStructure CreateReportingStructure(string employeeId)
        {
            ReportingStructure reportingStructure = new ReportingStructure();
            
            Employee employee = _employeeRepository.GetById(employeeId);    // get the specified employee
            reportingStructure.Employee = employee; // set the employee field for the ReportingStructure

            List<Employee> employeesWithReports = new List<Employee>() { employee };    // create a list of employees that have direct reports

            for(int i = 0; i < employeesWithReports.Count; i++) // iterate through list of employees that have direct reports
            {
                if(employeesWithReports[i].DirectReports != null && employeesWithReports[i].DirectReports.Count > 0)    // check if employee has direct reports and that the list is not empty
                {
                    foreach(Employee e in employeesWithReports[i].DirectReports)    // iterate through direct reports of current employee
                    {
                        reportingStructure.NumberOfReports++;  // increase number of direct reports
                        if (e.DirectReports != null && e.DirectReports.Count > 0)    // if the employee has direct reports, add them to the list
                        {
                            employeesWithReports.Add(e);
                        }
                    }
                }
            }
            return reportingStructure;
        }
    }
}
