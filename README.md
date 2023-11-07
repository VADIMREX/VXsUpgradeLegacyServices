# VXsUpgradeLegacyServices

Library for compatibility with legacy WCF/ASMX services.

# Plan
- [x] Get all classes with ServiceContract or WebService attributes
- [x] Parse .svc .asmx files
- [ ] Svc
  - [ ] Using data from ServiceContract attribute
    - [x] Name and Namepace
  - [ ] Using data from OperationContract attribute
    - [x] Name
  - [ ] Using data from DataContract attribute
    - [x] Name and Namepace
    - [ ] IsReference
  - [ ] Using data from DataMember attribute
    - [x] Name
    - [ ] Order
    - [ ] IsReference
  - [x] Serialize primitives, strings and classes with members of them
  - [x] Deserialize primiteves, strings and classes with members of them
  - [x] Serialize byte arrays
  - [x] Deserialize byte arrays
  - [ ] Serialize DateTime
  - [ ] Deserialize DateTime
  - [ ] Serialize Arrays and Lists
  - [ ] Deserialize Arrays and Lists
  - [ ] Serialize Dictionaries
  - [ ] Deserialize Dictionaries
  - [ ] JSON compatibility
- [ ] Asmx
  - [ ] Serialize primitives, strings and classes with members of them
  - [ ] Deserialize primiteves, strings and classes with members of them
  - [ ] Serialize DateTime
  - [ ] Deserialize DateTime
  - [ ] Serialize Arrays and Lists
  - [ ] Deserialize Arrays and Lists
  - [ ] Serialize Dictionaries
  - [ ] Deserialize Dictionaries
  - [ ] JSON compatibility
- [ ] Pages for testing
  - [x] in legacy SVC
  - [x] in lib SVC
  - [ ] in legacy ASMX
  - [ ] in lib ASMX
# How to run projects
## Sample legacy services
It can launched on IIS / IIS Express / XSP

### XSP
cd into SampleLegacyServices/bin folder and run:
```shell
xsp --applications=/:.,/:../
```