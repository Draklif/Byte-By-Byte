export interface Project {
    id: number;
    name: string;
    desc_short: string;
    date: string;
    authors: { name: string }[];
    tags: string[];
    format: string[];
    images: string[];
    links: string[];
    desc_long: string;
}
  