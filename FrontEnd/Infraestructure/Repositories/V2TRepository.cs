using Commons;
using Dapper;
using Data;
using Frontend.Entities;
using Frontend.Repositories;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Frontend.VTRepository
{
    public class V2TRepository : DapperRepository, IV2TRepository
    {
        public V2TRepository(IConfiguration configuration) : base(configuration){}

        public async Task<SpeechToTextRequest> SaveRequest(SpeechToTextRequest request)
        {
            DynamicParameters parameters = new();
            parameters.Add(EnumHelper.GetEnumDescription(EnumSpeechToTextRequestParameters.RequestFile), request.RequestFile);
            parameters.Add(EnumHelper.GetEnumDescription(EnumSpeechToTextRequestParameters.AudioLanguaje), request.AudioLanguaje);
            parameters.Add(EnumHelper.GetEnumDescription(EnumSpeechToTextRequestParameters.StateId), request.StateId);
            parameters.Add(EnumHelper.GetEnumDescription(EnumSpeechToTextRequestParameters.RequestedFromCountry), request.RequestedFromCountry);
            parameters.Add(EnumHelper.GetEnumDescription(EnumSpeechToTextRequestParameters.UserId), request.UserId);
            parameters.Add(EnumHelper.GetEnumDescription(EnumSpeechToTextRequestParameters.Extension), request.Extension);
            parameters.Add(EnumHelper.GetEnumDescription(EnumSpeechToTextRequestParameters.RequestId), request.RequestId);
            parameters.Add(EnumHelper.GetEnumDescription(EnumSpeechToTextRequestParameters.RequestDate), request.RequestDate);
            parameters.Add(EnumHelper.GetEnumDescription(EnumSpeechToTextRequestParameters.FileSize), request.FileSize);
            parameters.Add(EnumHelper.GetEnumDescription(EnumSpeechToTextRequestParameters.Response), request.Response);
            parameters.Add(EnumHelper.GetEnumDescription(EnumSpeechToTextRequestParameters.ResponseSize), request.ResponseSize);

            return await GetFirstAsync<SpeechToTextRequest>(DBParamHelper.BuilderFunction(DBParamHelper.EnumSchemas.DBO), parameters, CommandType.StoredProcedure);
        }
    }
}