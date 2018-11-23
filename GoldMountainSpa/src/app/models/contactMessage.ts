export class ContactMessage {
  Username: string;
  Email: string;
  PhoneNumber: string;
  Subject: string;
  Message: string;

  public constructor(init?:Partial<ContactMessage>) {
    Object.assign(this, init);
  }
}
