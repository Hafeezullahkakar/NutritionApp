import { Component, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { UserService } from '../../services/user.service';
import { NavbarComponent } from '../shared/navbar/navbar.component';
import { FoodService } from '../../services/food.service';
import { AuthService } from '../../services/auth.service';
import { CommonModule } from '@angular/common';
import { NgZone } from '@angular/core';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
  standalone: true,
  imports: [FormsModule, NavbarComponent, CommonModule],
})
export class DashboardComponent implements OnInit {
  user: any = {};
  foodInput: number = 0;

  availableFoods: any[] = [];
  selectedFoods: any[] = [];

  constructor(
    private userService: UserService,
    private foodService: FoodService,
    private authService: AuthService,
    private zone: NgZone // Inject NgZone
  ) {}

  ngOnInit(): void {
    this.fetchFoodItems();
    this.authService.currentUser$.subscribe((user) => {
      this.user = user;
      console.log('Current User in Dashboard:', user);
    });

    this.fetchUserData();
  }

  userProfile: any = {};
  fetchUserData(): void {
    this.userService.getUserDetails(this.user.nameid).subscribe({
      next: (data) => {
        this.userProfile = data;
        console.log('User data:', this.userProfile);
      },
      error: (error) => {
        console.error('Error fetching user data:', error);
      },
    });
  }

  fetchFoodItems(): void {
    this.foodService.getAllFoodItems().subscribe({
      next: (data) => {
        this.zone.run(() => {
          this.availableFoods = data.map((item) => ({
            id: item.id,
            name: item.name,
            type: item.type,
            amount: item.type === 'Countable' ? item.count : item.weight,
            fats: item.nutrient?.fats || 0,
            protein: item.nutrient?.protein || 0,
            carbs: item.nutrient?.carbs || 0,
            identifier: item.identifier,
          }));
        });
      },
      error: (error) => {
        console.error('Error fetching food items:', error);
      },
    });
  }

  addFoodToTable(foodId: number): void {
    const food = this.availableFoods.find((item) => item.id == foodId);

    if (food) {
      // Check if the food is already in the selected list
      const existingFood = this.selectedFoods.find(
        (item) => item.id === foodId
      );
      if (existingFood) {
        existingFood.quantity++; // Increment the quantity if already added
      } else {
        // Add food to the list with default quantity of 1
        this.selectedFoods.push({ ...food, quantity: 1 });
      }
    }
  }

  calculateNutrition(): void {
    const totalNutrition = this.selectedFoods.reduce(
      (totals, food) => ({
        protein: totals.protein + food.protein * food.quantity,
        fats: totals.fats + food.fats * food.quantity,
        carbs: totals.carbs + food.carbs * food.quantity,
        calories:
          totals.calories +
          (food.carbs * 4 + food.protein * 4 + food.fats * 9) * food.quantity,
      }),
      { protein: 0, fats: 0, carbs: 0, calories: 0 }
    );

    // Call API to update the user's daily intake
    this.userService
      .updateDailyIntake(this.userProfile.user.id, totalNutrition)
      .subscribe(
        (response) => {
          console.log('food items updated: ', response);
          this.fetchUserData();
          this.selectedFoods=[]
        },
        (error) => {
          console.error('Error updating daily intake:', error);
        }
      );
  }
}
