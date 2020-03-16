# triangle-search
Cherwell Challenge

Create a C# web api endpoint with optional JavaScript front end and post to Github for review.  Please provide your recruiter with the link
to the source code.  https://github.com/jeffrey-tucker-716/triangle-search

## Geometric Layouts

Calculate the triangle coordinates for an image with right triangles such that for a given row (A-F) and column (1-12), 
you can produce any of the triangles in the layout shown in CodingQuestion_2020_New.pdf.

Given the vertex coordinates, calculate the row and column for the triangle.

## Design of Data Transfer Object

The DTO for the project is a TriangleDetails class.  I leveraged the Point class in System.Drawing namespace just in case it would help.

    public class TriangleDetails
    {
        public string TriangleKey { get; set; } = string.Empty;
        public Point Vertex1 { get; set; } = Point.Empty;
        public Point Vertex2 { get; set; } = Point.Empty;
        public Point Vertex3 { get; set; } = Point.Empty;

        public bool IsValid
        {
            get
            {
                if (string.IsNullOrEmpty(TriangleKey) || (Vertex1 == Point.Empty && Vertex2 == Point.Empty && Vertex3 == Point.Empty))
                {
                    return false;
                }
                return true;
            }
        }

    }
}
## Routes for Endpoint
I wanted to adhere to RESTful architecture best practices so I created a resource of triangles.  TrianglesController
is an API controller (derived from ControllerBase)

The routes are
GET 
/triangles      retrieves them all
Example:
https://localhost:44392/triangles

GET 
/triangles/id:	retrieves just one, xor an empty invalid one, case insensitive for the search, but output in uppercase for the 
triangle key.
Example:
https://localhost:44392/triangles/A1    
{"triangleKey":"A1","vertex1":{"isEmpty":false,"x":0,"y":10},"vertex2":{"isEmpty":true,"x":0,"y":0},"vertex3":{"isEmpty":false,"x":10,"y":10},"isValid":true}

To avoid colliding with the route for all the triangles, I created an endpoint to look for a triangle based on its vertices
/triangles/search?v1=V1x,V1y&v2=V2x,V2y&v3=V3x,V3y
It is robust in the face of bad input.  It can be cached.
Example:
https://localhost:44392/triangles/search?v1=0,0&v2=10,0&v3=10,10
{"triangleKey":"A2","vertex1":{"isEmpty":true,"x":0,"y":0},"vertex2":{"isEmpty":false,"x":10,"y":0},"vertex3":{"isEmpty":false,"x":10,"y":10},"isValid":true}

All output is JSON.

Sample queries and output are found in TestQueriesToEndpoint.txt file

## The Service Layer

TriangleHunt is a simple class library containing the ITriangleResolver and TriangleResolver.  

To speed up the searches, I've created a singleton TriangleCache to look up any triangle by its key.
I could do some Dependency Injection of the TriangleCache, but you get the idea.  Even Michelangelo left David's head unfinished where
no one could see.

## Unit Testing

TriangleHunt.Tests is the NUnit testing project for the services layer.

TriangleSearchWebapp.Tests is an integration test project for the web api.





