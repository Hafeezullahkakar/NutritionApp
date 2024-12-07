import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiUrl = 'https://localhost:7060/api/Auth'; // Backend URL
  private foodApiURL = 'https://localhost:7060/api/Food'; // Backend URL

//  https://localhost:7060/api/Auth/register


  constructor(private http: HttpClient) {}

  getUserDetails(Id: number) {
    return this.http.get(`${this.apiUrl}/getuserdetails?Id=${Id}`);
  }

  updateMacroIntake(data: any) {
    return this.http.post(`${this.apiUrl}/macros`, data);
  }

  addFood(food: string, userId: number) {
    const requestData = {
      userId,
      food
    };
    return this.http.post(`${this.foodApiURL}/add-food`, requestData);
  }

  confirmFood(requestData: any) {
    return this.http.post(`${this.foodApiURL}/confirm-food`, requestData);
  }
  updateDailyIntake(userId: number, nutrition: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/updateDailyIntake?userId=${userId}`, nutrition);
  }
  


}
