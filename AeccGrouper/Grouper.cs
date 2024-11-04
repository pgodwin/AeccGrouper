using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AeccGrouper.Reference;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AeccGrouper
{
    /// <summary>
    /// Implements the Independent Hospital Pricing Authority Australian Emergency Care Classification grouper
    /// </summary>
    public class Grouper
    {
        public Grouper(IReferenceDataProviderService referenceDataService)
        {
            ReferenceDataService = referenceDataService; 
        }

        
        private const string AgeGroupOther = "Other";
        private const string AgeGroup0_4 = "0-4";
        private const string AgeGroup5_9 = "5-9";
        private const string AgeGroup10_14 = "10-14";
        private const string AgeGroup15_69 = "15-69";
        private const string AgeGroup70_74 = "70-74";
        private const string AgeGroup75_79 = "75-79";
        private const string AgeGroup80_84 = "80-84";
        private const string AgeGroup85Plus = "85+";

        /// <summary>
        /// Valid Vist Type Codes
        /// </summary>
        private readonly static string[] validVisitTypes = ["1", "2", "3", "5"];
        /// <summary>
        /// Valid Episode End Status Codes
        /// </summary>
        private readonly static string[] validEpisodeEndStatus = ["1", "2", "3", "5", "6"];
        /// <summary>
        /// Valid Triage Categories
        /// </summary>
        private readonly static string[] validTriageCategories = ["1", "2", "3", "4", "5"];
        /// <summary>
        /// Valid age groups
        /// </summary>
        private readonly static string[] validAgeGroups = [AgeGroup0_4, AgeGroup5_9, AgeGroup10_14, AgeGroup15_69, AgeGroup70_74, AgeGroup75_79, AgeGroup80_84, AgeGroup85Plus];

        /// <summary>
        /// Refence Data Provider Service
        /// </summary>
        public IReferenceDataProviderService ReferenceDataService { get; }


        /// <summary>
        /// Performs the grouping operation of the data
        /// </summary>
        /// <param name="episodeNumber"></param>
        /// <param name="triageCategory">
        /// Non-admitted patient emergency department service episode—triage category
        /// </param>
        /// <param name="episodeEndStatus">
        /// Non-admitted patient emergency department service episode—episode end status
        /// </param>
        /// <param name="visitType">
        /// Emergency department stay—type of visit to emergency department
        /// </param>
        /// <param name="ageYears">
        /// Derived from:
        /// • Person—date of birth
        /// • Non-admitted patient emergency department service
        ///   episode—clinical care commencement date
        /// </param>
        /// <param name="transportMode">
        /// Emergency department stay—transport mode (arrival)
        /// </param>
        /// <param name="principalDiaignosisShortCode">
        /// Emergency department stay—emergency department ICD-10-AM (10th edn) principal diagnosis short list code.
        /// </param>
        /// <param name="serviceDate">
        /// Clinical care commencement date/time
        ///  * Non-admitted patient emergency department service
        ///    episode—clinical care commencement date/time
        /// </param>
        public GrouperResult Group(
            string episodeNumber,
            string triageCategory,
            string episodeEndStatus,
            string visitType,
            string ageYearsString,
            string transportMode,
            string principalDiaignosisShortCode,
            string serviceDate)
        {
            // Include the source parameters in our result
            var result = new GrouperResult()
            {
                EpisodeNumber = episodeNumber,
                TriageCategory = triageCategory,
                EpisodeEndStatus = episodeEndStatus,
                TypeOfVisitToEd = visitType,
                AgeYears = ageYearsString,
                TransportMode = transportMode,
                PrincipalDiagnosisShortCode = principalDiaignosisShortCode,
                ServiceDate = serviceDate
            };


            // Remove any periods from the principal diagnosis short code
            principalDiaignosisShortCode = principalDiaignosisShortCode.Replace(".", string.Empty);

            // Validate the date input
            var validDateTime = DateTime.TryParse(serviceDate, out var clinicalCareCommencementDateTime);
            // Validate the age input
            var validAge = int.TryParse(ageYearsString, out var ageYears);
            

            // Source: [AECC Definitions Manual](https://www.ihacpa.gov.au/resources/aecc-definitions-manual)

            // The AECC grouper performs the following steps, in the order shown:
            //  1. Pre-ECDG processing (Level 1 classes)
            //  2. Assign ECDG and ECDG subcategories
            //  3. Determine the complexity score
            //  4. Allocate to final classes within ECDGs based on complexity score
            // 

            // ## Step 1: Pre-ECDG processing (Level 1 classes)
            // 
            // The grouper first uses the variables Visit type, Episode end status and Clinical care
            // commencement date and Clinical care commencement time to assign episodes to one of
            // four three classes, as shown in the Table below.
            //
            //
            // Table 2 Values and variables used for pre-ECDG classes
            //
            // AECC Class                                                 Grouping Logic
            // -----------------------------------------------------------------------------------------------------
            // E0001Z Not attended by a healthcare professional           Episode end status = 4 OR 8 OR
            //                                                            Episode end status = 5 AND Clinical care
            //                                                            commencement date / time is missing or invalid
            // -----------------------------------------------------------------------------------------------------
            // E0003Z Dead on arrival                                     Episode end status = 7 OR Visit type = 5
            // -----------------------------------------------------------------------------------------------------
            // E0002Z Planned return visit                                Visit type = 2
            // -----------------------------------------------------------------------------------------------------
            // 
            // The grouper logic first checks whether the *Episode end status* is `4 Did not wait to be attended
            // by a health care professional` or `8 Registered, advised of another health care service, and
            // left the emergency department without being attended by a health care professional` or `5
            // Left at own risk after being attended by a health care professional but before the nonadmitted
            // patient emergency department service episode was completed`.
            //
            // Values of `4` or `8` are automatically assigned to the end class `E0001Z Not attended by a healthcare
            // professional.`
            // Values of `5` are only assigned to this end class if they have a missing or invalid
            // Clinical care commencement date/time reported (i.e. an invalid/ missing date/time or a
            // valid date/time that precedes the Emergency department stay—presentation date/time).
            // The rationale for this is that if there is no Clinical care commencement date/time, the patient
            // was not attended by a healthcare professional.

            if (episodeEndStatus == "4" || episodeEndStatus == "8" ||
                (episodeEndStatus == "5" && !validDateTime))
            {
                result.AECC_EndClass = "E0001Z";
                return result;
            }

            // Assignment of an episode to `E0003Z Dead on arrival` is based on the episode having either an
            // Episode end type of `7 Dead on arrival`, and / or `Visit type 5 Dead on arrival`.
            if (episodeEndStatus == "7" || visitType == "5")
            {
                result.AECC_EndClass = "E0003Z";
                return result;
            }

            // Assignment of the episode to `E0002Z Planned return visit is based on the episode` having Visit
            // type `2 Return visit, planned`.
            if (visitType == "2")
            {
                result.AECC_EndClass = "E0002Z";
                return result;
            }

            // Episodes with a missing or invalid Visit type (i.e. a value that is not `1`, `2`, `3`, or `5`) are assigned to
            // error class `E9901Z Invalid visit type`.
            if (!validVisitTypes.Contains(visitType))
            {
                result.AECC_EndClass = "E9901Z";
                return result;
            }

            // All other episodes with a valid Visit type are allocated to the ‘Emergency presentations’
            // category, which is processed further at step 3 (pending further edits at Step 2).
            // Table 3 shows this.
            
            // At this point, we should be left with Emergency presentations...so can continue.

            // ## Step 2: Assign ECDG and ECDG subcategories
            //
            // Following the pre-ECDG processing (Step 1), for all remaining episodes the grouper checks
            // whether a valid Principal diagnosis short list code has been assigned. If the Principal diagnosis
            // short list code is missing, the episode is assigned to the error class `E99002 Missing principal
            // diagnosis` short list code.
            // If the Principal diagnosis short list code is invalid (i.e. it is not a code
            // within IHPA’s Emergency Department ICD-10-AM Principal Diagnosis Short List) the episode is
            // assigned to the error class E99003 Invalid principal diagnosis short list code.

            if (string.IsNullOrWhiteSpace(principalDiaignosisShortCode))
            {
                result.AECC_EndClass = "E99002Z"; // Missing principal diagnosis shortlist code
                return result;
            }
            
            // Check we have a valid code in our reference data
            var shortListCode = ReferenceDataService.GetECDGCode(principalDiaignosisShortCode);
            
            // If no code is returend, we can assume it's invalid and can return the error class
            if (shortListCode == null)
            {
                result.AECC_EndClass = "E9903Z"; // Invalid principal diagnosis 
                return result;
            }

            // For remaining episodes, the grouper allocates each episode to an ECDG based on the
            // Principal diagnosis short list code. Appendix C contains the map of Principal diagnosis short
            // list codes to ECDGs
            result.ECDG = shortListCode.ECDG_code;


            // In preparation for step 3, the grouper logic also assigns an ECDG subcategory. The ECDG
            // subcategories are also groupings of the Principal diagnosis short list codes, but at a more
            // detailed level. Appendix B lists the ECDG subcategories by ECDG and ECC. The maps from
            // the Principal diagnosis short list codes to the ECDG subcategories is at Appendix B.
            result.ECDG_Subgroup = shortListCode.ECDG_sub;

            // ## Step 3: Determine the complexity score
            //
            // Following assignment to an ECDG, the grouper uses the values assigned to the episode for
            // each of the variables shown in Table 4, to calculate a complexity score. Episodes with
            // missing or invalid values for any of these variables are assigned to the error class E9904Z Other
            // error.

            // Table 4 Variables and values used to assign the complexity score
            //
            // Variable            Values used by the grouper            How values are derived
            // -----------------------------------------------------------------------------------------------------
            // ECDG subcategory    See list in Appendix B. Appendix C    Assigned by grouper, derived from 
            //                     contains the map from the             Emergency department stay—emergency
            //                     Principal diagnosis short list code   department ICD-10-AM (10th edn) 
            //                                                           principal diagnosis short list code (METeOR
            //                                                           identifier 681646). See Step 2
            // -----------------------------------------------------------------------------------------------------
            // Transport arrival   - Ambulance, air/helicopter           Where value = 1
            // mode                - Other                               All other values
            // -----------------------------------------------------------------------------------------------------
            // Episode end         - Admitted                            Where value = 1
            // status              − Departed                            Where value = 2
            //                     − Referred to another hospital        Where value = 3
            //                     − Died in emergency                   Where value = 6
            //                       department
            //                     − Left at own risk                    Where value = 5
            // -----------------------------------------------------------------------------------------------------
            // Triage category     - 1                                   Where value = 1
            //                     - 2                                   Where value = 2
            //                     - 3                                   Where value = 3
            //                     - 4                                   Where value = 4
            //                     - 5                                   Where value = 5
            // -----------------------------------------------------------------------------------------------------
            // Age group           - 0-4                                 Assigned by grouper, derived from:
            //                     - 5-9                                  * Person—date of birth AND
            //                     - 10-14                                * Non-admitted patient emergency
            //                     - 15-69                                  department service episode—clinical
            //                     - 70-74                                  care commencement date
            //                     - 75-79
            //                     - 80-84
            //                     - 85+
            // -----------------------------------------------------------------------------------------------------

            var ageBracket = GetAgeBracket(ageYears);
            result.AgeBracket = ageBracket;

            // Perform the validation, if any fail return the error class E9904Z Other error
            if (string.IsNullOrWhiteSpace(transportMode) ||
                string.IsNullOrWhiteSpace(episodeEndStatus) ||
                string.IsNullOrWhiteSpace(triageCategory) ||
                string.IsNullOrWhiteSpace(ageBracket) || 
                // I haven't implemented a validator for transportMode
                !validAgeGroups.Contains(ageBracket) ||
                !validEpisodeEndStatus.Contains(episodeEndStatus) ||
                !validTriageCategories.Contains(triageCategory) ||
                !validVisitTypes.Contains(visitType))
            {
                result.AECC_EndClass = "E9904Z";
                return result;
            }

            // The complexity score is calculated by applying a coefficient related to the reported values
            // for each of the variables in the Table.The coefficients for each variable vary across ECDGs.
            // Appendix D has four tables that specify the coefficients as follows:
            // • Table D1: Intercept and ECDG subcategory
            // • Table D2: Transport mode(arrival), Episode end status and Triage category
            // • Table D3: Age group
            // • Table D4: Interactions(Episode end status of admitted and Triage category, and
            //   Episode end status of admitted and Age group).

            var d1Intercept = ReferenceDataService.GetIntercept(result.ECDG, result.ECDG_Subgroup);

            result.InterceptScore = d1Intercept?.Intercept ?? 0;
            result.SubInterceptScore = d1Intercept?.Intercept_sub ?? 0;
            var triageCategoryValue = ReferenceDataService.GetTriageCategory(result.ECDG, episodeEndStatus, transportMode, triageCategory);
            result.TransportModeScore = triageCategoryValue?.TransportModeValue ?? 0;
            result.EpisodeEndStatusScore = triageCategoryValue?.EpisodeEndStatusValue ?? 0;
            result.TriageCategoryScore = triageCategoryValue?.TriageCategoryValue ?? 0;
            
            result.AgeGroupScore = ReferenceDataService.GetAgeGroup(result.ECDG, ageBracket)?.AgeGroupValue ?? 0;
            
            var interactionRecord = ReferenceDataService.GetInteraction(result.ECDG, triageCategory, ageBracket);
            result.AgeInteractionScore = interactionRecord?.AgeValue ?? 0;
            result.TriageInteractionScore = interactionRecord?.TriageValue ?? 0;

            // The InteractionScore lookup only applies to Episode end status = admitted
            if (episodeEndStatus != "1")
            {
                result.InteractionScore = 0;
            }
            else
            {
                result.InteractionScore = result.AgeInteractionScore + result.TriageInteractionScore;
            }


            // Table 5 provides an example of how the coefficients and the values of the relevant variables
            // are used to calculate the complexity score, in this instance for an episode assigned to ECDG
            // E0490 Respiratory disorders, other.
            //
            // The sum of the coefficients that apply to the episode are calculated to yield the predicted value.
            
            var predictedValue = result.InterceptScore +
                                 result.SubInterceptScore +
                                 result.TransportModeScore +
                                 result.EpisodeEndStatusScore +
                                 result.TriageCategoryScore +
                                 result.AgeGroupScore +
                                 result.InteractionScore;

            // These are then re - scaled to generate the complexity score.
            // In this example, the predicted value is 7.09, and the re - scaled complexity
            // score is 6.19.The formula for rescaling the predicted value is:
            //
            // score = (exp(predicted_value) - 713) / 166 + 3.26
            //
            // Where: 
            // * exp(predicted_value) is the exponent of the predicted value derived from the
            //   regression model(taking an exponent is the ‘reverse’ of taking a log)
            // * 713 is the mean of exp(predicted_value)
            // * 166 is two times the standard deviation of exp(score)
            // * 3.26 reflects the minimum value of the previous steps, with a small adjustment to
            //   ensure new observations are all greater than zero.

            double score = (Math.Exp(Math.Round(predictedValue, 4, MidpointRounding.AwayFromZero)) - 713d) / 166d + 3.26d;
            if (score < 0) // make sure they're all greater than 0
                score = 0;

            
            result.ScaledComplexityScore = score;
            result.PredicatedValue = predictedValue;

            // ## 4. Allocate to final classes within ECDGs based on complexity score
            // In the final step the grouper uses the complexity scores and the thresholds specified in
            // Appendix E to assign episodes to a final class. Some ECDGs are not split, in which case the
            // final class has a suffix of ‘Z’ assigned. Where the ECDG is split, the final classes are assigned a
            // suffix of A for the most complex episodes within the ECDG, B for the next level of complexity,
            // and so on, depending on the number of splits (up to four).
            //
            // ---
            // title Figure 4 AECC grouper logic: Steps 2, 3 and 4
            // -- -
            // flowchart TB
            //     A[\1 /] --> B{ Missing principal diagnosis short list code ?}
            //     B-- Yes-- > C(E9902Z Missing principal diagnosis short list code)
            //     B-- No-- > D{ Invalid principal diagnosis short list code ?}
            //     D-- Yes-- > E(E9903Z Invalid principal diagnosis short list code)
            //     D-- No-- > F{ "Invalid/missing transport (arrival) mode, episode end status, triage category, age group?"}
            //     F-- Yes-- > G(E9904Z Other error)
            //     F-- No-- > H[Assign to one of 69 ECDGs]
            //     H-- > I[Calculate complexity score]
            //     I-- > J[Apply thresholds to assign episode to end class within ECDG]
            //     J --> K(AECC end class)

            var finalClass = ReferenceDataService.GetFinalClass(result.ECDG, score);
            result.AECC_EndClass = finalClass.FirstOrDefault()?.AECC_class ?? string.Empty;

            return result;

        }

        public string GetAgeBracket(int? ageYears)
        {
            return ageYears switch
            {
                < 0 => "Other",
                <= 4 => "0-4",
                <= 9 => "5-9",
                <= 14 => "10-14",
                <= 69 => "15-69",
                <= 74 => "70-74",
                <= 79 => "75-79",
                <= 84 => "80-84",
                < 119 => "85+",
                _ => "Other",
            };
        }
    }
}
