export enum InstitutionType {
  Bank,
  Credit,
  Insur
}

export class Institution {
  Name: string;
  IsSupported: boolean;
  Credentials: Array<string>;
  Type: InstitutionType;

  public constructor(init?:Partial<Institution>) {
    Object.assign(this, init);
  }
}



