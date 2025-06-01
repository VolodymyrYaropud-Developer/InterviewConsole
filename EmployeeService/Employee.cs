﻿using System.Collections.Generic;
using System.Runtime.Serialization;

namespace EmployeeService
{
    [DataContract]
    public class Employee
    {
        [DataMember]
        public int ID { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int? ManagerID { get; set; }
        [DataMember]
        public bool Enable { get; set; }
        [DataMember]
        public List<Employee> Employees { get; set; }
    }

    public class Response
    {
        public Employee Employee { get; set; }
    }
}