
 
 

 

/// <reference path="Enums.ts" />

declare namespace Sihapp.WebProject.Models {
	interface ConsultVM {
		AppointmentNumber: number;
		ClinicMenName: string;
		ID: System.Guid;
		IsFinished: boolean;
		Notes: string;
		Number: number;
		PatientBirthDate: Date;
		PatientName: string;
		StartDate: Date;
	}
	interface CustomFieldsVM {
		AllowAllEntities: boolean;
		FieldText: string;
		FieldType: number;
		RelatedID: string;
		Value: string;
	}
	interface TestModel {
		MyProperty: number;
		MyProperty2: string;
	}
}
declare namespace Sihapp.WebProject.Models.Appointments {
	interface AppointmentBackendDataVM {
		AppointmentStatusCodes: Sihapp.WebProject.Models.Common.TextValueVM[];
		ClinicMen: Sihapp.WebProject.Models.Common.TextValueVM[];
		EndDate: Date;
		Option: number;
		PatientList: Sihapp.WebProject.Models.Common.TextValueVM[];
		StartDate: Date;
		UserType: number;
	}
	interface AppointmentItemsVM {
		Address: string;
		AppointmentDate: Date;
		AppointmentID: System.Guid;
		CanStartConsult: boolean;
		ClinicMenName: string;
		ConsultID: System.Guid;
		HasActiveConsult: boolean;
		Number: number;
		PatientName: string;
		SelectedStatus: Sihapp.WebProject.Models.Common.TextValueVM;
		Status: number;
		StatusText: string;
	}
	interface GroupedAppointmentVM {
		Date: Date;
		Items: Sihapp.WebProject.Models.Appointments.AppointmentItemsVM[];
	}
	interface PageableAppointmentsVM {
		Groups: Sihapp.WebProject.Models.Appointments.GroupedAppointmentVM[];
	}
}
declare namespace Sihapp.WebProject.Models.ClinicMen {
	interface ClinicMenSettingsVM {
		Birthdate: Date;
		Cellphone: string;
		RFC: string;
		ToleranceMinutes: number;
	}
}
declare namespace Sihapp.WebProject.Models.Common {
	interface PaginationVM {
		Page: number;
		PageSize: number;
		SearchTerm: string;
	}
	interface TextValueVM {
		Text: string;
		Value: any;
	}
}
declare namespace Sihapp.WebProject.Models.Consult {
	interface ConsultImageVM {
		CreationDate: Date;
		Description: string;
		ID: System.Guid;
		Image: number[];
	}
}
declare namespace Sihapp.WebProject.Models.CustomFields {
	interface ExtraFieldsVM {
		ClinicID: System.Guid;
		FieldText: string;
		FieldType: number;
		Id: System.Guid;
		Value: System.Guid;
	}
}
declare namespace Sihapp.WebProject.Models.Manager {
	interface SihappAccountsPagedVM {
		AccountCount: number;
		items: Sihapp.WebProject.Models.Manager.SihappAccountsVM[];
		TotalPages: number;
	}
	interface SihappAccountsVM {
		ClinicID: System.Guid;
		ClinicImages: number;
		ClinicName: string;
		Consults: number;
		Email: string;
		MasterUser: string;
		Number: number;
		PatientsQty: number;
		Phone: string;
		User: string;
	}
}
declare namespace Sihapp.WebProject.Models.Patients {
	interface PageablePatientsVM {
		PatientItems: Sihapp.WebProject.Models.Patients.PatientItemVM[];
		PatientsQty: number;
		TotalPages: number;
	}
	interface PatientItemVM {
		Address: string;
		BirthDate: Date;
		HasUser: boolean;
		ID: System.Guid;
		Name: string;
		Number: number;
		Phone: string;
	}
}
declare namespace Sihapp.WebProject.Models.Patients.Expedient {
	interface PatientExpedientViewBagVM {
		Age: number;
		Birthdate: Date;
		ConsultQty: number;
		EndDateFilter: Date;
		GeneralConsultQty: number;
		ID: System.Guid;
		LastName1: string;
		LastName2: string;
		Name: string;
		Notes: string;
		Number: number;
		Phone: string;
		StartDateFilter: Date;
	}
}
declare namespace Sihapp.WebProject.Models.Patients.Payments {
	interface PatientPaymentBackendDataVM {
		ClinicMen: Sihapp.WebProject.Models.Common.TextValueVM[];
		EndDate: Date;
		Keyword: string;
		Patients: Sihapp.WebProject.Models.Common.TextValueVM[];
		RemissionID: System.Guid;
		StartDate: Date;
	}
	interface PatientPaymentsVM {
		ClinicMen: string;
		ConsultNumber: string;
		GrandTotal: number;
		PatientName: string;
		Payed: number;
		Pending: number;
	}
}
declare namespace Sihapp.WebProject.Models.RemissionNotes {
	interface PagedPatientPaymentVM {
		GrandTotal: number;
		PaymentItems: Sihapp.WebProject.Models.RemissionNotes.PatientPaymentItemVM[];
		TotalPages: number;
		TotalPayed: number;
	}
	interface PagedRemissionNotes {
		RemissionList: Sihapp.WebProject.Models.RemissionNotes.RemissionNotesItems[];
		TotalGrandAmount: number;
		TotalPages: number;
		TotalPayed: number;
		TotalPending: number;
	}
	interface PagedRemissionPaymentsVM {
		PaymentItems: Sihapp.WebProject.Models.RemissionNotes.RemissionNotesItems;
		TotalPayed: number;
	}
	interface PatientPaymentItemVM {
		ApplicationDate: string;
		LastBalance: number;
		Notes: string;
		PaymentAmount: number;
		PaymentID: System.Guid;
	}
	interface PatientRemissionNoteBackendDataVM {
		ClinicManName: string;
		ConsultNumber: string;
		CreationDate: Date;
		GrandTotal: number;
		Notes: string;
		PatientName: string;
		Payed: number;
		Pending: number;
		RemissionID: System.Guid;
		RemissionNumber: number;
	}
	interface RemissionNotesItems {
		ClinicMen: string;
		ConsultNumber: string;
		CreationDate: Date;
		GrandTotal: number;
		ID: System.Guid;
		Number: number;
		PatientName: string;
		Payed: number;
		Pending: number;
	}
	interface RemissionPaymentItem {
		Amount: number;
		Comments: string;
		Id: System.Guid;
		PaymentDate: Date;
	}
}
declare namespace Sihapp.WebProject.Models.Users {
	interface NewUserVM {
		BirhtDate: Date;
		Email: string;
		LastName1: string;
		LastName2: string;
		Name: string;
		Password: string;
		Phone: string;
		UserName: string;
		UserType: number;
	}
	interface PageableUsersVM {
		TotalPages: number;
		UserItems: Sihapp.WebProject.Models.Users.UserItemsVM[];
	}
	interface UserItemsVM {
		AppointmentCount: number;
		CreationDate: Date;
		EntityID: System.Guid;
		ID: string;
		Name: string;
		Type: number;
		TypeName: string;
		UserName: string;
	}
}
declare namespace System {
	interface Guid {
	}
}


