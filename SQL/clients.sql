create table clients (
	id int not null primary key identity,
	name varchar(100) not null,
	email varchar(150) not null unique,
	phone varchar(20) null,
	address varchar(100) null,
	created_at datetime not null default current_timestamp
)

insert into clients (name, email, phone, address)
values
('Bill Gates', 'bill.gates@microsoft.com', '+23423423425', 'New Your, USA'),
('Elon Musk', 'elon.musk@spacex.com', '+5632346642', 'Florida, USA'),
('Will Smith', 'will.smith@gmail.com', '+2566069754', 'California, USA'),
('Bob MArley', 'bob.marley@gmail.com', '+4312523312', 'Texas, USA'),
('Cristiano Ronaldo', 'christiano.ronaldo@gmail.com', '+0432242233', 'Manchester, England'),
('Borris Johnson', 'borris.johnson@gmail.com', '+423544531', 'London, England')