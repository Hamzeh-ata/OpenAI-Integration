import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ChatBotService {
  constructor(private http:HttpClient) { }

  private apiUrl = 'https://localhost:7219/api/ChatGpt';

  sendMessage(messageText:string):Observable<any>{
    const body = { Question: messageText };
    return this.http.get<any>(`${this.apiUrl}/ask`, {params: body });
 }
 chat(messages: { role: string, content: string }[]): Observable<any> {
  return this.http.post<any>(`${this.apiUrl}/chat`, { messages });
}

}
