import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
//import {config} from '../../assets/config/app.config';

const apiUrl : string = "https://localhost:7261/"; //http://192.168.29.223/calculator/api/";
//const apiUrl : string = "http://3.108.219.234/SunSparkal/";
//const apiKey = config.apiKey;
@Injectable({
    providedIn: 'root' // or specify a module where it should be provided
  })
export class SharedService {

    constructor(private httpClient: HttpClient) { }

    customGetApi(api: string): Observable<ApiResponse> {
        return this.httpClient.get<any>(apiUrl + api).pipe(map(t => t), catchError(err => throwError(err)));
    }
    customGetApi1<T>(api: string): Observable<T> {
        return this.httpClient.get<T>(apiUrl + api).pipe(
          map((response) => response), // You can directly return the response as data
          catchError((err) => throwError(err)) // Handle errors
        );
      }

    customPostApi(api: string, data: any): Observable<ApiResponse> {
        return this.httpClient.post<any>(apiUrl + api, data).pipe(map(t => t),catchError(err => throwError(err)));
    }

    JsonConvert<T>(jsonString: string): T {
        const obj = JSON.parse(jsonString);
        return obj as T;
    }
}

export class ApiResponse {
    success: boolean = false;
    statusCode: number;
    sessage: string = '';
    data: any
}


  