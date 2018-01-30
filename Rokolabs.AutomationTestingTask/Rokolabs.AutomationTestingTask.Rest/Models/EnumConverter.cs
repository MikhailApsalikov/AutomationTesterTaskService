using System;
using Rokolabs.AutomationTestingTask.Entities.Enums;

namespace Rokolabs.AutomationTestingTask.Rest.Models
{
	public static class EnumConverter
	{
		public static InteractionTypes ToInteractionType(string str)
		{
			switch (str)
			{
				case "Conference":
					return InteractionTypes.Conference;
				case "Ndr":
					return InteractionTypes.Ndr;
				case "IndustryEvent":
					return InteractionTypes.IndustryEvent;
				case "FieldTrips":
					return InteractionTypes.FieldTrips;
				case "CompanyEvents":
					return InteractionTypes.CompanyEvents;
				case "Sales":
					return InteractionTypes.Sales;
				case "Other":
					return InteractionTypes.Other;
				default:
					throw new ArgumentException("Invalid Interaction Type");
			}
		}

		public static MeetingTypes ToMeetingType(string str)
		{
			switch (str)
			{
				case "OneByOne":
					return MeetingTypes.OneByOne;
				case "TwoByOne":
					return MeetingTypes.TwoByOne;
				case "ThreeByOne":
					return MeetingTypes.ThreeByOne;
				case "Annotated":
					return MeetingTypes.Annotated;
				case "Call":
					return MeetingTypes.Call;
				case "Email":
					return MeetingTypes.Email;
				case "Group":
					return MeetingTypes.Group;
				case "EmailGroup":
					return MeetingTypes.EmailGroup;
				case "Im":
					return MeetingTypes.Im;
				case "PrivateMeeting":
					return MeetingTypes.PrivateMeeting;
				case "Survey":
					return MeetingTypes.Survey;
				case "VideoConference":
					return MeetingTypes.VideoConference;
				case "VoiceMail":
					return MeetingTypes.VoiceMail;
				case "Na":
					return MeetingTypes.Na;
				case "Other":
					return MeetingTypes.Other;
				default:
					throw new ArgumentException("Invalid Meeting Type");
			}
		}

		public static Sectors ToSectors(string str)
		{
			switch (str)
			{
				case "ConsumerDiscretionary":
					return Sectors.ConsumerDiscretionary;
				case "Energy":
					return Sectors.Energy;
				case "Financials":
					return Sectors.Financials;
				case "HealthCare":
					return Sectors.HealthCare;
				case "Industrials":
					return Sectors.Industrials;
				case "InformationTechnology":
					return Sectors.InformationTechnology;
				case "Macro":
					return Sectors.Macro;
				case "Materials":
					return Sectors.Materials;
				case "Other":
					return Sectors.Other;
				case "RealEstate":
					return Sectors.RealEstate;
				case "TelecommunicationServices":
					return Sectors.TelecommunicationServices;
				case "Utilities":
					return Sectors.Utilities;
				default:
					throw new ArgumentException("Invalid Sector");
			}
		}

		public static AddressTypes ToAddressType(string str)
		{
			switch (str)
			{
				case "None":
					return AddressTypes.None;
				case "Home":
					return AddressTypes.Home;
				case "Away":
					return AddressTypes.Away;
				case "Both":
					return AddressTypes.Both;
				default:
					throw new ArgumentException("Invalid Address Type");
			}
		}
	}
}