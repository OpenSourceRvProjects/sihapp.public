select *from aspNetUsers where Id not in (select UserID from ClinicMen) and UserType = 0 

select *from aspNetUsers where Id  not in (select UserID from Patients) and UserType = 1

select *from aspNetUsers where Id not in (select UserID from AuxiliarPersonal) and UserType = 2

delete from aspNetUsers where Id not in (select UserID from ClinicMen)  and UserType = 0 

delete from aspNetUsers where Id not in (select UserID from Patients) and UserType = 1

delete from aspNetUsers where Id not in (select UserID from AuxiliarPersonal)and UserType = 2

select *from aspNetUsers