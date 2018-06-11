import * as helper from "../shared/uuid";

export class UserProfile  {
  Id: string;
  PassportId: string;
  Email: string;
  FirstName: string;
  LastName: string;
  FullName: string;

  public constructor(init?:Partial<UserProfile>) {
    Object.assign(this, init);
  }
}

export const DEV_USER: UserProfile = {
  Id: helper.defaultUserUuid(),
  PassportId: '394837348',
  Email: 'dev.dev@mail.com',
  FirstName: 'Developer',
  LastName: 'Tester',
  FullName: 'Demo User',
};
