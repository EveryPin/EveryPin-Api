using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Shared.Dtos.Common.Blob;

namespace ExternalLibraryService;

public class BlobHandlingService
{
    private readonly BlobServiceClient _blobService;
    private readonly BlobContainerClient _blobContainer;

    #region 세팅
    public BlobHandlingService(string accessKey, string storageAccountName, string containerName)
    {

        _blobService = GetBlobServiceClient(storageAccountName, accessKey);
        _blobContainer = GetBlobContainerClient(containerName);
    }

    private BlobServiceClient GetBlobServiceClient(string storageAccountName, string accessKey)
    {
        var credential = new StorageSharedKeyCredential(storageAccountName, accessKey);
        string blobUri = $"https://{storageAccountName}.blob.core.windows.net";

        BlobServiceClient client = new BlobServiceClient(new Uri(blobUri), credential);

        return client;
    }

    public BlobContainerClient GetBlobContainerClient(string containerName)
    {
        return _blobService.GetBlobContainerClient(containerName);
    }

    //public BlobClient GetBlobClient(string blobName)
    //{
    //    return _blobContainer.GetBlobClient(blobName);
    //}
    #endregion

    /// <summary>
    /// Blob Storage 컨테이너 내 모든 Blob List 반환
    /// </summary>
    /// <returns></returns>
    public async Task<List<BlobDto>> ListAsync()
    {
        List<BlobDto> files = new List<BlobDto>();

        await foreach (var file in _blobContainer.GetBlobsAsync())
        {
            string uri = _blobContainer.Uri.ToString();
            var name = file.Name;
            var fullUri = $"{uri}/{name}";

            files.Add(new BlobDto
            {
                Uri = uri,
                Name = name,
                ContentType = file.Properties.ContentType,
            });
        }

        return files;
    }

    /// <summary>
    /// Blob Storage 컨테이너에 이미지 파일 업로드
    /// </summary>
    /// <param name="blob"></param>
    /// <returns></returns>
    public async Task<BlobResultDto> UploadAsync(IFormFile blob)
    {
        BlobResultDto response = new BlobResultDto();
        BlobClient client = _blobContainer.GetBlobClient(blob.FileName);
        string contentType = "image/jpeg";

        if(blob is not null)
        {
            await using (Stream? data = blob.OpenReadStream())
            {
                await client.UploadAsync(data); // 업로드

                BlobHttpHeaders headers = new BlobHttpHeaders { ContentType = contentType };

                await client.SetHttpHeadersAsync(headers);  // ContentType 변경
            }

            response.Message = $"{blob.FileName} 업로드 완료";
            response.Error = false;
            response.Blob.Uri = client.Uri.AbsoluteUri;
            response.Blob.Name = client.Name;
            response.Blob.ContentType = contentType;
        }
        else
        {
            response.Message = $"업로드 실패";
            response.Error = true;
            response.Blob.Uri = null;
            response.Blob.Name = null;
            response.Blob.ContentType = null;
        }

        return response;
    }

    public async Task<BlobResultDto> UploadPostPhotoAsync(int postPhotoId, IFormFile blob)
    {
        string blobFileName = $"PostPhoto_{postPhotoId}";
        BlobResultDto response = new BlobResultDto();
        BlobClient client = _blobContainer.GetBlobClient(blobFileName);
        string contentType = "image/jpeg";

        if (IsImageFile(blob))
        {
            await using (Stream? data = blob.OpenReadStream())
            {
                await client.UploadAsync(data); // 업로드

                BlobHttpHeaders headers = new BlobHttpHeaders { ContentType = contentType };

                await client.SetHttpHeadersAsync(headers);  // ContentType 변경
            }

            response.Message = $"{blob.FileName} 업로드 완료";
            response.Error = false;
            response.Blob.Uri = client.Uri.AbsoluteUri;
            response.Blob.Name = client.Name;
            response.Blob.ContentType = contentType;
        }
        else
        {
            response.Message = "이미지 파일만 업로드할 수 있습니다.";
            response.Error = true;
            response.Blob.Uri = null;
            response.Blob.Name = null;
            response.Blob.ContentType = null;
        }

        return response;
    }

    private bool IsImageFile(IFormFile file)
    {
        // 이미지 파일 확장자 목록
        string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp" };

        // 파일 확장자 확인
        string fileExtension = Path.GetExtension(file.FileName).ToLower();

        // 이미지 파일인지 확인
        return imageExtensions.Contains(fileExtension);
    }

    /// <summary>
    /// Blob Storage 컨테이너에 특정 파일 반환하기
    /// </summary>
    /// <param name="blobFileName"></param>
    /// <returns></returns>
    public async Task<BlobDto?> DownloadAsync(string blobFileName)
    {
        BlobClient file = _blobContainer.GetBlobClient(blobFileName);

        if (await file.ExistsAsync())
        {
            var data = await file.OpenReadAsync();
            var content = await file.DownloadContentAsync();

            string name = blobFileName;
            string contentType = content.Value.Details.ContentType;

            return new BlobDto { Content = data, Name = name, ContentType = contentType };
        }

        return null;
    }

    /// <summary>
    /// Blob Storage 컨테이너 내 특정 파일 삭제하기
    /// </summary>
    /// <param name="blobFileName"></param>
    /// <returns></returns>
    public async Task<BlobResultDto> DeleteAsync(string blobFileName)
    {
        BlobClient file = _blobContainer.GetBlobClient(blobFileName);
        await file.DeleteAsync();

        return new BlobResultDto { Error = false, Message = $"{blobFileName} 삭제 완료" };
    }
}
