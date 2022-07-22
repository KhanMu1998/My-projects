package com.example.servingwebcontent.domain;
import javax.persistence.*;
import java.util.ArrayList;
import java.util.List;

@Entity
public class House {
    @Id
    private String address;
    private Integer floorsNumber;
    private Integer entrancesNumber;
    private Integer apartsNumber;

    @OneToMany(mappedBy = "address", fetch = FetchType.EAGER, orphanRemoval = false)
    private List<Apart> listApart = new ArrayList<>();

    public House() {

    }

    public House(String address, Integer floorsNumber, Integer entrancesNumber, Integer apartsNumber) {
        this.address = address;
        this.floorsNumber = floorsNumber;
        this.entrancesNumber = entrancesNumber;
        this.apartsNumber = apartsNumber;
    }

    public String getAddress() {
        return address;
    }

    public void setAddress(String address) {
        this.address = address;
    }

    public Integer getFloorsNumber() {
        return floorsNumber;
    }

    public void setFloorsNumber(Integer floorsNumber) {
        this.floorsNumber = floorsNumber;
    }

    public Integer getEntrancesNumber() {
        return entrancesNumber;
    }

    public void setEntrancesNumber(Integer entrancesNumber) {
        this.entrancesNumber = entrancesNumber;
    }

    public Integer getApartsNumber() {
        return apartsNumber;
    }

    public void setApartsNumber(Integer apartsNumber) {
        this.apartsNumber = apartsNumber;
    }
}
