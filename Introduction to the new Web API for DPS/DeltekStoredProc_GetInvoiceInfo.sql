if exists (select 1 from INFORMATION_SCHEMA.ROUTINES where ROUTINE_NAME = 'DeltekStoredProc_GetInvoiceInfo')
begin
    drop procedure DeltekStoredProc_GetInvoiceInfo
end
go

create procedure DeltekStoredProc_GetInvoiceInfo
    @Invoice varchar(12)
as
begin
    select  
		  billInvMaster.Invoice,
		  billInvMaster.MainWBS1,
		  billInvMaster.InvoiceDate,
		  billInvMaster.MainName,
		  billInvMaster.Description,
		  PR.LongName as ProjectName,
		  CL.Name as ClientName
    from	  billInvMaster
		  join PR on billInvMaster.MainWBS1 = PR.WBS1 and PR.WBS2 = ' ' and PR.WBS3 = ' '
		  join CL on PR.BillingClientID = CL.ClientID
    where	  Invoice = @Invoice
    group by billInvMaster.Invoice,
		  billInvMaster.MainWBS1,
		  billInvMaster.InvoiceDate,
		  billInvMaster.MainName,
		  billInvMaster.Description,
		  PR.LongName,
		  CL.Name

    select
		  section,
		  sum(BaseAmount) as BaseAmount,
		  sum(FinalAmount) as FinalAmount
		  
    from	  billInvSums
    where	  Invoice = @Invoice and ArrayType = 'I'
		  group by section

end
go


--exec DeltekStoredProc_GetInvoiceInfo '0000189'
