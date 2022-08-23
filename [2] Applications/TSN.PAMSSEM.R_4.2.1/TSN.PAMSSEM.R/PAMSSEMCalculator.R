##################################################
# PAMSSEM CALCULATOR
# Version   : 1.0
# Author    : MUSTAFA TOSUN, https://mustafatosun.net
# Date      : 2022-08-24
# Licence   : NONE
##################################################


windowWidth <- 150
Alternatives <- NA
IsOrdinal <- NA
Attributes <- NA
DecisionMatrix <- NA
Differences <- NA
Indices <- NA
Ds <- NA
OutrankingIndices <- NA
ConcordanceIndices <- NA
LocalDiscordanceIndices <- NA
OutrankingDegrees <- NA
EnteringFlows <- NA
LeavingFlows <- NA
NetFlows <- NA



writeObject <- function(x, title = "", wl = F) {
  t <- as.character(title)[1]
  writeLines(c(paste(c(rep(" ", windowWidth - nchar(t)), t), collapse = ""), paste(rep("-", windowWidth), collapse = "")))
  if (nchar(t) > 0) {
    if (wl)
      writeLines(x)
    else
      print(x)
  }
  writeLines(rep("", 3))
}

writeHeader <- function(title) {
  t <- as.character(title)[1]
  hr <- paste(rep("-", windowWidth), collapse = "")
  spaces <- rep(" ", as.integer((windowWidth - 2 - nchar(t)) / 2))
  writeLines(c(hr, paste(c("|", spaces, t, spaces, ifelse(nchar(t) %% 2 != 0, " ", ""), "|"), collapse = ""), hr, ""))
}

InitializePAMSSEM <- function(alternatives, isOrdinal, attributes_Name, attributes_Weight, attributes_q, attributes_p, attributes_v, attributes_y, attributes_isPositive, decisionMatrix) {
  writeHeader("AŞAMA 0 - PROBLEM OLUŞTURMA")
  
  Alternatives <<- as.vector(alternatives, mode = "character")
  IsOrdinal <<- ifelse(isOrdinal, T, F)
  Attributes <<- data.frame(
    Name = as.vector(attributes_Name, mode = "character"),
    Weight = as.vector(attributes_Weight, mode = "numeric"),
    q = as.vector(attributes_q, mode = "numeric"),
    p = as.vector(attributes_p, mode = "numeric"),
    v = as.vector(attributes_v, mode = "numeric"),
    y = ifelse(IsOrdinal, NA, as.vector(attributes_y, mode = "numeric")),
    IsPositive = as.vector(attributes_isPositive, mode = "logical"))
  #colnames(Attributes) <<- c("Ad", "Ağırlık", "Kayıtsızlık Eşiği", "Tercih Eşiği", "Reddetme Eşiği", "y Parametresi", "Pozitiflik")
  DecisionMatrix <<- matrix(decisionMatrix, nrow = length(Alternatives), ncol = length(Attributes$Name))
  rownames(DecisionMatrix) <<- Alternatives
  colnames(DecisionMatrix) <<- Attributes$Name
  
  writeObject(Alternatives, "ALTERNATİFLER")
  writeObject(ifelse(IsOrdinal, "Ordinal", "Kardinal"), "ORDİNALLLİK", T)
  writeObject(Attributes, "ÖZELLİKLER")
  writeObject(DecisionMatrix, "KARAR MATRİSİ")
}

SolvePAMSSEM_Stage1 <- function() {
  writeHeader("AŞAMA 1 - YEREL GEÇİŞ İNDİSİ")
  
  Differences <<- array(rep(NA, length(Alternatives) * length(Alternatives) * length(Attributes$Name)), dim = c(length(Alternatives), length(Alternatives), length(Attributes$Name)), dimnames = list(Alternatives, Alternatives, paste(Attributes$Name, " (", ifelse(Attributes$IsPositive, "+", "-"), ")", sep = "")))
  Indices <<- array(rep(NA, length(Alternatives) * length(Alternatives) * length(Attributes$Name)), dim = c(length(Alternatives), length(Alternatives), length(Attributes$Name)), dimnames = list(Alternatives, Alternatives, Attributes$Name))
  OutrankingIndices <<- matrix(rep(NA, length(Alternatives) * length(Alternatives)), nrow = length(Alternatives), dimnames = list(Alternatives, Alternatives))
  
  for (i in 1:length(Alternatives))
    for (j in 1:length(Alternatives)) {
      if (i == j)
        next
      sum <- 0;
      for (k in 1:length(Attributes$Name)) {
        Differences[i, j, k] <<- (DecisionMatrix[i, k] - DecisionMatrix[j, k]) * ifelse(Attributes$IsPositive[k], 1, -1)
        if (Differences[i, j, k] <= -Attributes$p[k])
          sum <- sum + (Indices[i, j, k] <<- 0)
        else if (Differences[i, j, k] >= -Attributes$q[k])
          sum <- sum + (Indices[i, j, k] <<- 1)
        else if (-Attributes$p[k] < Differences[i, j, k] - Attributes$q[k] && Attributes$p[k] >= Attributes$q[k] && Attributes$q[k] >= 0)
          sum <- sum + (Indices[i, j, k] <<- (Differences[i, j, k] - Attributes$p[k]) / (Attributes$p[k] - Attributes$q[k]))
        else {
          sum <- sum + (Indices[i, j, k] <<- NaN)
          break
        }
      }
      OutrankingIndices[i, j] <<- sum
    }
  
  writeObject(Differences, "FARKLAR")
  writeObject(Indices, "İNDİSLER")
  writeObject(OutrankingIndices, "YEREL GEÇİŞ İNDİSLERİ")
}

SolvePAMSSEM_Stage2 <- function() {
  writeHeader("AŞAMA 2 - AHENK İNDİSİ")
  
  # ...
  
  writeObject(ConcordanceIndices, "AHENK İNDİSİ")
}

SolvePAMSSEM_Stage3 <- function() {
  writeHeader("AŞAMA 3 - YEREL UYUMSUZLUK İNDİSİ")
  
  # ...
  
  writeObject(Ds, "D")
  writeObject(LocalDiscordanceIndices, "YEREL UYUMSUZLUK İNDİSİ")
}

SolvePAMSSEM_Stage4 <- function() {
  writeHeader("AŞAMA 4 - GEÇİŞ DERECESİ")
  
  # ...
  
  writeObject(OutrankingDegrees, "GEÇİŞ DERECESİ")
}

SolvePAMSSEM_Stage5 <- function() {
  writeHeader("AŞAMA 5 - GİRİŞ VE ÇIKIŞ AKIŞLARI")
  
  # ...
  
  writeObject(EnteringFlows, "GİRİŞ AKIŞLARI")
  writeObject(LeavingFlows, "ÇIKIŞ AKIŞLARI")
}

SolvePAMSSEM_Stage6 <- function() {
  writeHeader("AŞAMA 6 - NET AKIŞLAR")
  
  # ...
  
  writeObject(NetFlows, "NET AKIŞLAR")
}

SolvePAMSSEM_Finalize1 <- function() {
  writeHeader("NİHAİ SIRALAMA - PAMSSEM I")
  
  # ...
  
  writeObject(NA)
}

SolvePAMSSEM_Finalize2 <- function() {
  writeHeader("NİHAİ SIRALAMA - PAMSSEM II")
  
  # ...
  
  writeObject(NA)
}