# **Computer Manager**

Project is used to report **final project** of subject **OOP development method**, UIT.

The **final report** can see at [here](https://docs.google.com/document/d/1X7Q053TTHynktHo-zW1_wAA5DkvkaaYXtVeazrp8kuU/edit?usp=sharing).

## **About team**

Team contains 4 member:
- [Vũ Ngọc Thạch](https://github.com/vungocthach) - MSSV: 19520266 - Back-end developer
- [Nguyễn Phạm Duy Bằng](https://github.com/bang2001vl) - MSSV: 19520397 - Back-end developer
- [Huỳnh Thị Minh Nhực](https://github.com/HuynhThiMinhNhuc) - MSSV: 19521973 - Front-end developer
- [Võ Thị Thủy Tiên](https://github.com/thuytien192) - MSSV: 19520296 - Front-end developer

__Time start:__ 06/09/2021

__Time end:__ 04/12/2021

## **Main features**

- Manage product (image, name, price by VND, description)
- Manage category (root - child)
- Orders (automatic billing): 
  - with specific **filter**
  - get bonus point for every buying
  - automatic create information of new customer
- Repair & warranty:
  - automatic check warranty of a seri's product
  - about 3 status: fixing, fixed, paid
- History: search between 2 days
- Statistic: 
  - after month by **chart** and **table**
  - automatic get top selling of month
  - compare **revenue**, number **new customer**, **order**, **bill** with previous month.
- Setting rule: 
  - point standard
  - shop information (name, phone, address)
  - team support information

## **About software**
Language: C#

Platform: WPF

Pattern: MVVM

UI design: [Figma](https://www.figma.com/file/7fVZ5pulBdXmResyc0J4II/Sales-computer-manager?node-id=0%3A1)

Database: SQL Server - Azure
- Structure: refer at [draw.io](https://drive.google.com/file/d/1P_V46n2boeFPj24-ylzhZp--Ef0c2YAd/view?usp=sharing)

Framework:
- [Entity Framework 6.0](https://www.entityframeworktutorial.net/entityframework6/introduction.aspx) (DB-first)
- [Material Design](https://material.io/)

---
## **Some UI of app**
**Main tab**

<img src="UI%20image/main_tab.gif" alt='drawing' height='400'/>

**Order tab**

<img src="UI%20image/sale_tab.gif" alt='drawing' height='400'/>

With full information of cart:

<img src="UI%20image/sale_tab_full.png" alt='drawing' height='400'/>

**Payment view**

<img src="UI%20image/bill_view.png" alt='drawing' height='500'/>

**Repair tab**

<img src="UI%20image/repair_tab.gif" alt='drawing' height='400'/>

- Interact with bill repair:

<table>
    <td>
        <img src="UI%20image/repair_tab_add.png" alt='drawing'/>
    </td>
    <td>
        <img src="UI%20image/repair_tab_edit.png" alt='drawing'/>
    </td>
</table>

**Product tab**

- Main view products:
  
<img src="UI%20image/product_tab.png" alt='drawing' height='400'/>

- Interact with product:

<table>
    <td>
        <img src="UI%20image/product_tab_add.png" alt='drawing'/>
    </td>
    <td>
        <img src="UI%20image/product_tab_edit.png" alt='drawing'/>
    </td>
</table>

**Category tab**

<img src="UI%20image/category_tab.png" alt='drawing' height='400'/>

- Detail of category

<img src="UI%20image/category_tab_edit.png" alt='drawing' height='400'/>

**History tab**

<img src="UI%20image/bill_tab.png" alt='History' height='400'/>

**Customer tab**

<img src="UI%20image/customer_tab.png" alt='History' height='400'/>

- Detail of customer:

<img src="UI%20image/customer_tab_edit.png" alt='History' height='400'/>

**Statistic tab**

<img src="UI%20image/statistic_tab.png" alt='Statistic_detail' height='400'/>

**Information tab**

<img src="UI%20image/setting_tab.gif" alt='drawing' height='400'/>

**Some another UI**

- Billing PDF

<img src="UI%20image/pdf.png" alt='pdf' height="400"/>

- Messagebox

<img src="UI%20image/mess_bill_success.png" alt='drawing' height="200"/>