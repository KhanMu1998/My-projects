package com.example.servingwebcontent.domain;

import javax.persistence.*;
import java.util.ArrayList;
import java.util.List;

@Entity
public class Person {
    @Id
    private String fio;
    private String passport;
    private String phone;

    @OneToMany(mappedBy = "owner", fetch = FetchType.EAGER, orphanRemoval = false)
    private List<Apart> listApart = new ArrayList<>();


    public Person() {

    }
    public Person(String fio, String passport, String phone) {
        this.fio = fio;
        this.passport = passport;
        this.phone = phone;
    }

    public String getFio() {
        return fio;
    }

    public void setFio(String fio) {
        this.fio = fio;
    }

    public String getPassport() {
        return passport;
    }

    public void setPassport(String passport) {
        this.passport = passport;
    }

    public String getPhone() {
        return phone;
    }

    public void setPhone(String phone) {
        this.phone = phone;
    }
}
