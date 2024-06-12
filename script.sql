CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

CREATE TABLE "Businesses" (
    "BusinessId" uuid NOT NULL,
    "OwnerId" uuid NOT NULL,
    "Name" text NOT NULL,
    CONSTRAINT "PK_Businesses" PRIMARY KEY ("BusinessId")
);

CREATE TABLE "Services" (
    "ServiceId" uuid NOT NULL,
    "Title" text NOT NULL,
    "EstimatedDurationInMinutes" integer NOT NULL,
    "Price" numeric NOT NULL,
    "IsHomeService" boolean NOT NULL,
    "BusinessId" uuid NOT NULL,
    CONSTRAINT "PK_Services" PRIMARY KEY ("ServiceId")
);

CREATE TABLE "Bookings" (
    "Id" uuid NOT NULL,
    "BusinessId" uuid NOT NULL,
    "ServiceId" uuid NOT NULL,
    "Costs" numeric NOT NULL,
    "CustomerId" uuid NOT NULL,
    "BookingDate" timestamp with time zone NOT NULL,
    "Status" integer NOT NULL,
    "AppointmentLocation" jsonb NOT NULL,
    CONSTRAINT "PK_Bookings" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Bookings_Businesses_BusinessId" FOREIGN KEY ("BusinessId") REFERENCES "Businesses" ("BusinessId") ON DELETE CASCADE,
    CONSTRAINT "FK_Bookings_Services_ServiceId" FOREIGN KEY ("ServiceId") REFERENCES "Services" ("ServiceId") ON DELETE CASCADE
);

CREATE INDEX "IX_Bookings_BusinessId" ON "Bookings" ("BusinessId");

CREATE INDEX "IX_Bookings_ServiceId" ON "Bookings" ("ServiceId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20240524130453_Initial', '8.0.5');

COMMIT;

