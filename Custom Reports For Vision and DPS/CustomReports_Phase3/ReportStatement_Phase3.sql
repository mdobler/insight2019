DECLARE @ActiveCompany varchar( 10 ) = ' '
DECLARE @ActivePeriod int = 200506
DECLARE @CustRegularProjectsOnly char(1) = 'Y'

SELECT
	EM.Employee , EM.FirstName , EM.MiddleName , EM.LastName , 
	PR.WBS1 , PR.Name AS ProjectName , 
	LEVEL2.WBS2 , ISNULL( LEVEL2.Name , '' )AS WBS2Name , 
	LEVEL3.WBS3 , ISNULL( LEVEL3.Name , '' )AS WBS3Name , 
	SUM( LD.RegHrs + LD.OvtHrs + LD.SpecialOvtHrs )AS TotalHours , 
	SUM( CASE WHEN LD.LaborCode LIKE CFGBillMain.NonBillLaborCode THEN 0 ELSE LD.RegHrs + LD.OvtHrs + LD.SpecialOvtHrs END )AS BillableHours , 
	SUM( CASE WHEN LD.LaborCode LIKE CFGBillMain.NonBillLaborCode THEN LD.RegHrs + LD.OvtHrs + LD.SpecialOvtHrs ELSE 0 END )AS NonBillableHours
FROM EM
	JOIN EmployeeCustomTabFields ON EM.Employee = EmployeeCustomTabFields.Employee
	JOIN LD ON EM.Employee = LD.Employee
	JOIN PR ON LD.WBS1 = PR.WBS1 AND PR.WBS2 = ' ' AND PR.WBS3 = ' '     
	LEFT JOIN PR LEVEL2 ON LD.WBS1 = LEVEL2.WBS1 AND LD.WBS2 = LEVEL2.WBS2 AND LEVEL2.WBS3 = ' '
	LEFT JOIN PR LEVEL3 ON LD.WBS1 = LEVEL3.WBS1 AND LD.WBS2 = LEVEL3.WBS2 AND LD.WBS3 = LEVEL3.WBS3 
	LEFT JOIN CFGBillMain ON CFGBillMain.Company = @ActiveCompany
	CROSS APPLY CFGSystem 
	CROSS APPLY CFGFormat
WHERE
	( CFGSystem.MulticompanyEnabled = 'N' OR SUBSTRING( EM.Org , CFGFormat.Org1Start , Org1Length ) = @ActiveCompany )
	AND LD.Period = @ActivePeriod
	AND (@CustRegularProjectsOnly = 'N' or PR.ChargeType = 'R')
	/***EXTEND WHERE CLAUSE***/
GROUP BY
	EM.Employee , EM.FirstName , EM.MiddleName , EM.LastName , 
	PR.WBS1 , PR.Name , LEVEL2.WBS2 , ISNULL( LEVEL2.Name , '' ) , LEVEL3.WBS3 , ISNULL( LEVEL3.Name , '' )
ORDER BY
	EM.Employee , PR.WBS1 , LEVEL2.WBS2 , LEVEL3.WBS3	