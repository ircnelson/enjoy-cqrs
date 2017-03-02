// The MIT License (MIT)
//
// Copyright (c) 2016 Nelson Corrêa V. Júnior
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using EnjoyCQRS.EventSource.Projections;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace EnjoyCQRS.EventStore.MongoDB
{
    public class MongoProjection
    {
        [BsonId]
        public string Id => $"{Category}:{ProjectionId}";

        [BsonElement("pid")]
        public Guid ProjectionId { get; private set; }

        [BsonElement("category")]
        public string Category { get; private set; }

        [BsonElement("projection")]
        public BsonDocument Projection { get; private set; }

        public static MongoProjection Create(ISerializedProjection projection)
        {
            return new MongoProjection(projection.ProjectionId, projection.Category, BsonDocument.Parse(projection.Projection));
        }

        private MongoProjection(Guid projectionId, string category, BsonDocument projection)
        {
            ProjectionId = projectionId;
            Category = category;
            Projection = projection;
        }
    }
}