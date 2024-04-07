export class Room {
  constructor(
    public roomNumber: number,
    public availability: boolean,
    public price: number,
    public roomType: string,
    public description: string,
    public amenities: string[],
    public images: string[],
    public id?: number
  ) {}
}
