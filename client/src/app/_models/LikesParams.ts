export class LikedParams{
    predicate: string;
    pageNumber = 1;
    pageSize = 5;

    constructor(predicate: string) {
        this.predicate = predicate;
    }
}