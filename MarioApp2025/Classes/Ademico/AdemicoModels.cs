using System;
using System.Collections.Generic;

namespace MarioApp2025
{
    public class AdemicoModels
    {
        public class ViesApproximate
        {
            public string Name { get; set; }
            public string Street { get; set; }
            public string PostalCode { get; set; }
            public string City { get; set; }
            public string CompanyType { get; set; }
            public int MatchName { get; set; }
            public int MatchStreet { get; set; }
            public int MatchPostalCode { get; set; }
            public int MatchCity { get; set; }
            public int MatchCompanyType { get; set; }
        }
        public class VatResponse
        {
            public bool IsValid { get; set; } = false;
            public DateTime RequestDate { get; set; }
            public string UserError { get; set; }
            public string Name { get; set; }
            public string Address { get; set; }
            public string RequestIdentifier { get; set; }
            public string OriginalVatNumber { get; set; }
            public string VatNumber { get; set; }
            public ViesApproximate ViesApproximate { get; set; }
        }
        public class Contact
        {
            public string ContactType { get; set; }
            public string Name { get; set; }
            public string PhoneNumber { get; set; }
            public string Email { get; set; }
        }
        public class LegalEntityDetails
        {
            public bool PublishInPeppolDirectory { get; set; }
            public string Name { get; set; }
            public string CountryCode { get; set; }
            public string GeographicalInformation { get; set; }
            public string WebsiteUrl { get; set; }
            public List<Contact> Contacts { get; set; }
            public string AdditionalInformation { get; set; }
        }
        public class PeppolIdentifier
        {
            public string Scheme { get; set; }
            public string Identifier { get; set; }
        }
        public class PeppolRegistration
        {
            public PeppolIdentifier PeppolIdentifier { get; set; }
            public List<string> SupportedDocuments { get; set; }
            public bool PublishInSmp { get; set; }
        }
        public class CreateLegalEntityModel
        {
            public LegalEntityDetails LegalEntityDetails { get; set; }
            public List<PeppolRegistration> PeppolRegistrations { get; set; }
        }
    }
}
