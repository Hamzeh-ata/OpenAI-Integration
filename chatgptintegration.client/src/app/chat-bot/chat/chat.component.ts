import { Component, ElementRef, ViewChild } from '@angular/core';
import { ChatBotService } from '../services/chat-bot.service';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';

interface Message {
  text: string;
  sanitizedText: SafeHtml;
  sender: string;
}

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css'],
})
export class ChatComponent {
  messages: Message[] = [];
  @ViewChild('messageInput') messageInputRef!: ElementRef<HTMLInputElement>;
  messageInputValue: string = '';
  isTyping: boolean = false;

  constructor(
    private chatBotService: ChatBotService,
    private sanitizer: DomSanitizer
  ) {}

  sendMessage(messageText: string) {
    if (!messageText.trim()) {
      return;
    }

    const newMessage: Message = {
      text: messageText,
      sanitizedText: this.sanitizeText(messageText),
      sender: 'Hamzeh',
    };

    this.messages.push(newMessage);
    this.messageInputValue = '';
    this.messageInputRef.nativeElement.value = '';
    this.isTyping = true;

    const chatMessages = [
      { role: 'system', content: 'You are a risk manager assistant.' },
      ...this.messages.map((m) => ({
        role: m.sender === 'Hamzeh' ? 'user' : 'assistant',
        content: m.text,
      })),
      { role: 'user', content: messageText },
    ];

    this.chatBotService.chat(chatMessages).subscribe((response) => {
      const newMessage: Message = {
        text: response.text,
        sanitizedText: this.sanitizeText(response.text),
        sender: response.sender,
      };
      this.messages.push(newMessage);
      this.isTyping = false;
    });
  }

  sanitizeText(text: string): SafeHtml {
    const textWithLineBreaks = text.replace(/\n/g, '<br>');
    return this.sanitizer.bypassSecurityTrustHtml(textWithLineBreaks);
  }
}
