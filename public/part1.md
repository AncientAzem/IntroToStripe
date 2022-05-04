---
marp: true
theme: gaia
_class: lead
style: |
  h1 {
    text-align: center
  }
paginate: true
backgroundColor: #fff
backgroundImage: url('./background2.png')
---

![bg](./background.png)

# Introduction to Stripe

Start accepting online payments today!

---

#

![bg left:30% 70%](./headshot.png)

# Who am I?

Hey there! My name is Colin Lowel and I'm a Fullstack Developer here locally.

I specialize in .NET Web API and Front-end Web Development using Firebase's tooling for authentication, database hosting, and cloud messaging.


---

#

# What is Stripe?

Stripe is a payment infrastructure that allows developers and business alike to easily start accepting payments for services that takes the burden of security and processing off of the back of the business and developers.

Used by companies ranging from Google, Slack, Shopify, Lyft, Instacart and more!

---
#

# Stripe's Product Offering

- Invoicing
- Online Payment Processing (Payments)
- Subscription Management (Billing)
- In Person Payments (Terminal)
- Fraud Protection (Radar)
- 3rd/Multi Party Payment Platform (Connect)
- Identity Verification (Identity)
- Startup Management Program (Atlas)
<!-- Atlas: Allows you to form a LLC or C Corporation with ease -->

---
#

# Core Platform Pricing

For access to the platform in a production setup, you'll pay the following:

- 2.9% + 30¢ per Successful Card Charge
- 2.7% + 4¢ per In Person Card Transaction
- 0.8% on ACH Transfers (Max of $5)
- 0.4% per Invoice (First 25 Free)
- 0.5% on Billing (i.e. Subscription Plan)

---
#

# Additional Service Pricing

- $1.50 per Identity Verification (50¢ for SSN Lookup)
- 0.5% per Transaction for Local Tax Collection
- 0.4% per Transaction for Chargeback Protection
- 0.25% of Account Volume for Multiparty Connect Accounts
- 25¢ per Customer Card Auto-Update
- Free Fraud Protection (Requires Standard Plan)
- $500 one-time fee for Atlas Accounts

<!--
Chargeback Protection: Stripe will cover both the disputed amount and any dispute fees—no evidence submission required.

Connect Accounts: Multiparty accounts are Express/Custom accounts that allow you to build a custom mutli-party platform solutions

Card Auto Update: Allows stripe to automatically update the billing card data for your customers should their bank issue a new card and the provider is participating -->

---

#

# What I'll Be Covering

- Basic Payment Processing
- Product Management
- Invoice Generation
- Stripe API Usage