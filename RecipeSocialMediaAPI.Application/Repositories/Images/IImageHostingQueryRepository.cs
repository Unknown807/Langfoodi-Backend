﻿using RecipeSocialMediaAPI.Application.DTO.ImageHosting;

namespace RecipeSocialMediaAPI.Application.Repositories.ImageHosting;
public interface IImageHostingQueryRepository
{
    public CloudinarySignatureDTO? GenerateClientSignature(string? publicId);
    public CloudinarySignatureDTO? GenerateClientSignature(List<string> publicIds);
}
